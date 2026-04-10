using Database.Models;
using Repository.Interfaces;
using Services.Interfaces;
using Shared.Dtos;
using Shared.Dtos.QueryFilters;
using Shared.Dtos.ReportsDtos;
using Shared.Dtos.WorkerDtos;
using Shared.Helper;
using Shared.Mapping;

namespace Services.Implementations
{
    public class WorkerService : IWorkerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        public WorkerService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<ViewWorkerDto?> CreateWorkerAsync(CreateWorkerDto createWorkerDto)
        {
            var worker = createWorkerDto.ToWorker();

            var userId = _currentUserService.GetCurrentUserId();
            if(string.IsNullOrWhiteSpace(userId))
                throw new InvalidOperationException("UserID not found in authentication context.");

            worker.UserId = userId;

            await _unitOfWork.Workers.AddAsync(worker);
            await _unitOfWork.SaveChangesAsync();

            return worker.ToWorkerDto();
        }

        public async Task<ViewWorkerDto?> DeleteWorkerAsync(int id)
        {
            var worker = await _unitOfWork.Workers.GetWorkerByIdAsync(id);

            if (worker == null)
                return null;

            _unitOfWork.Workers.Delete(worker);
            await _unitOfWork.SaveChangesAsync();
            return worker.ToWorkerDto();
        }

        public async Task<IEnumerable<ViewWorkerDto>> GetAllWorkersAsync()
        {
            var workers = await _unitOfWork.Workers.GetWorkersAsync();

            return workers.Select(w => w.ToWorkerDto());    
        }

        public async Task<ViewWorkerDto?> GetWorkerByIdAsync(int id)
        {
            var worker = await _unitOfWork.Workers.GetWorkerByIdAsync(id);

            if (worker == null)
                return null;

            return worker.ToWorkerDto();
        }

        public async Task<ViewWorkerDto?> UpdateWorkerAsync(int id, UpdateWorkerDto updateWorkerDto)
        {
            var worker = await _unitOfWork.Workers.GetWorkerByIdAsync(id);

            if (worker == null)
                return null;

           worker.UpdateWorker(updateWorkerDto);
            await _unitOfWork.SaveChangesAsync();

            return worker.ToWorkerDto();
        }

        public async Task<BaseWeeklyPaymentDto?> GetWeeklyPaymentAsync(CreateWeeklyPaymentDto dto, bool addToExpense)
        {
            var worker = await _unitOfWork.Workers.GetWorkerByIdAsync(dto.Worker_Id);
            if (worker == null) return null;

            var payment =  worker.Worker_Type == WorkerType.Girls
                ? await GetWeeklyPaymentForGirlsAsync(dto, worker)
                : await GetWeeklyPaymentForAllWorkersExceptGirlsAsync(dto, worker);

            if (addToExpense)
                await AddPaymentToExpensesAsync(payment);

            await _unitOfWork.SaveChangesAsync();

            return payment;
        }

        private async Task AddPaymentToExpensesAsync(BaseWeeklyPaymentDto payment)
        {
            var expense = payment.ToCreateExpenseDto().ToExpense();

            await _unitOfWork.Expenses.AddAsync(expense);
        }

        private decimal GetPricePerUnitByWorkerType(WorkerType workerType, Model model)
        {
            return workerType switch
            {
                WorkerType.Cutter => model.Price_Cutter,
                WorkerType.Stitcher => model.Price_Stitcher,
                WorkerType.Ironer => model.Price_Ironer,
                _ => 0
            };
        }

        private async Task<(decimal totalAdvances, decimal totalDeductions)>
            GetAdvancesAndDeductionsAsync(int workerId)
        {
            var items = await _unitOfWork.AdvanceAndDeductions
                .FindAsync(ad => ad.Worker_Id == workerId);
            var itemIds = items.Select(i => i.Id).ToList();

            var totalAdvances = items.Where(ad => ad.Type == AdvanceOrDeduction.Advance && !ad.IsUsed)
                                       .Sum(ad => ad.Amount);

            var totalDeductions = items.Where(ad => ad.Type == AdvanceOrDeduction.Deduction && !ad.IsUsed)
                                       .Sum(ad => ad.Amount);

            await _unitOfWork.AdvanceAndDeductions.MakeAdvanceAndDeductionUsed(itemIds);

            return (totalAdvances, totalDeductions);
        }

        private async Task<ViewWeeklyPaymentDto?>
            GetWeeklyPaymentForAllWorkersExceptGirlsAsync(CreateWeeklyPaymentDto createWeeklyPaymentDto, Worker worker)
        { 

            var modelIds = createWeeklyPaymentDto.WorkItems.Select(wi => wi.Model_Id).ToList();
            var models =(modelIds.Count > 0)
                ? await _unitOfWork.Models.GetModelsByIdsAsync(modelIds)
                : new List<Model>();

            var lines = new List<PaymentLineDto>();


            var (totalAdvances, totalDeductions) = await GetAdvancesAndDeductionsAsync(worker.Id);

            foreach (var wi in createWeeklyPaymentDto.WorkItems)
            {
                var model = models.FirstOrDefault(m => m.Id == wi.Model_Id);

                if (model != null)
                {
                    var pricePerUnit = GetPricePerUnitByWorkerType(worker.Worker_Type, model);
                    lines.Add(new PaymentLineDto
                    {
                        Model_Name = model.Model_Name,
                        Quantity = wi.Quantity,
                        PricePerUnit = pricePerUnit,
                        LineTotal = pricePerUnit * wi.Quantity
                    });
                }
            }

            var totalEarned = lines.Sum(l => l.LineTotal);

            var paymentReport = new ViewWeeklyPaymentDto
            {
                Worker_Id = worker.Id,
                Worker_Name = worker.Worker_Name,
                Worker_Type = worker.Worker_Type.ToString(),
                Lines = lines,
                TotalQuantity = lines.Sum(l => l.Quantity),
                TotalEarned = totalEarned,
                TotalAdvances = totalAdvances,
                TotalDeductions = totalDeductions,
                NetPayment = totalEarned - totalAdvances - totalDeductions
            };

            return paymentReport;
        }

        private async Task<BaseWeeklyPaymentDto?>
            GetWeeklyPaymentForGirlsAsync(CreateWeeklyPaymentDto createWeeklyPaymentDto,Worker worker)
        {
            var totalEarned = createWeeklyPaymentDto.ManualSalary ?? 0;

           var (totalAdvances, totalDeductions) = await GetAdvancesAndDeductionsAsync(worker.Id);

            var paymentReport = new ViewWeeklyPaymentForGirlsDto { 
                Worker_Id = worker.Id,
                Worker_Name = worker.Worker_Name,
                Worker_Type = worker.Worker_Type.ToString(),
                TotalEarned = totalEarned,
                TotalAdvances = totalAdvances,
                TotalDeductions = totalDeductions,
                NetPayment = totalEarned - totalAdvances - totalDeductions
            };

            return paymentReport;
        }

        public async Task<IEnumerable<ViewEnumDto>> GetWorkerTypesAsync()
        {
            var types = EnumHelper.GetEnumList<WorkerType>();

            return types;
        }

        public async Task<IEnumerable<ViewWorkerDto>> GetWorkersByFilterAsync(WorkerFilter workerFilter)
        {
            var workers = await _unitOfWork.Workers.GetWorkersByFilterAsync(
                workerName: workerFilter.WorkerName,
                type: workerFilter.Type);

            return workers.Select(w => w.ToWorkerDto());
        }
    }
}
