using Database.Models;
using Microsoft.Extensions.Logging;
using Repository.Interfaces;
using Services.Interfaces;
using Shared;
using Shared.Dtos;
using Shared.Dtos.QueryFilters;
using Shared.Dtos.ReportsDtos;
using Shared.Dtos.WorkerDtos;
using Shared.Helper;
using Shared.Interfaces;
using Shared.Mapping;

namespace Services.Implementations
{
    public class WorkerService : IWorkerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<WorkerService> _logger;

        public WorkerService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, ILogger<WorkerService> logger)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<ViewWorkerDto?> CreateWorkerAsync(CreateWorkerDto createWorkerDto)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Creating new worker", userContext);

                var worker = createWorkerDto.ToWorker();

                if (worker == null)
                    throw new InvalidOperationException("Failed to map worker DTO to entity.");

                var userId = _currentUserService.GetCurrentUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    throw new InvalidOperationException("UserID not found in authentication context.");

                worker.UserId = userId;

                await _unitOfWork.Workers.AddAsync(worker);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("{userContext} - Worker created successfully. Worker ID: {Id}", userContext, worker.Id);

                return await GetWorkerByIdAsync(worker.Id);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("{userContext} - Validation error: {Message}", userContext, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error creating worker: {Message}", userContext, ex.Message);
                throw;
            }
        }

        public async Task<ViewWorkerDto?> DeleteWorkerAsync(int id)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Deleting worker {Id}", userContext, id);

                var worker = await _unitOfWork.Workers.GetWorkerByIdAsync(id);

                if (worker == null)
                {
                    _logger.LogWarning("{userContext} - Worker {Id} not found", userContext, id);
                    return null;
                }

                _unitOfWork.Workers.Delete(worker);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("{userContext} - Worker {Id} deleted successfully", userContext, id);

                return await GetWorkerByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error deleting worker: {Message}", userContext, ex.Message);
                throw;
            }
        }

        public async Task<ViewWorkerDto?> GetWorkerByIdAsync(int id)
        {
            try
            {
                var worker = await _unitOfWork.Workers.GetWorkerByIdAsync(id);

                if (worker == null)
                    return null;

                return worker.ToWorkerDto();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving worker {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<ViewWorkerDto?> UpdateWorkerAsync(int id, UpdateWorkerDto updateWorkerDto)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Updating worker {Id}", userContext, id);

                var worker = await _unitOfWork.Workers.GetWorkerByIdAsync(id);

                if (worker == null)
                {
                    _logger.LogWarning("{userContext} - Worker {Id} not found", userContext, id);
                    return null;
                }

                worker.UpdateWorker(updateWorkerDto);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("{userContext} - Worker {Id} updated successfully", userContext, id);

                return await GetWorkerByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error updating worker: {Message}", userContext, ex.Message);
                throw;
            }
        }

        public async Task<BaseWeeklyPaymentDto?> GetWeeklyPaymentAsync(CreateWeeklyPaymentDto dto, bool addToExpense)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Calculating weekly payment for worker {Id}", userContext, dto.Worker_Id);

                var worker = await _unitOfWork.Workers.GetWorkerByIdAsync(dto.Worker_Id);
                if (worker == null)
                {
                    _logger.LogWarning("{userContext} - Worker {Id} not found", userContext, dto.Worker_Id);
                    return null;
                }

                var payment = worker.Worker_Type == WorkerType.Girls
                    ? await GetWeeklyPaymentForGirlsAsync(dto, worker)
                    : await GetWeeklyPaymentForAllWorkersExceptGirlsAsync(dto, worker);

                if (addToExpense)
                    await AddPaymentToExpensesAsync(payment);

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("{userContext} - Weekly payment calculated successfully for worker {Id}", userContext, dto.Worker_Id);

                return payment;
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error calculating weekly payment: {Message}", userContext, ex.Message);
                throw;
            }
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
            GetWeeklyPaymentForAllWorkersExceptGirlsAsync(
            CreateWeeklyPaymentDto createWeeklyPaymentDto, Worker worker)
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
            try
            {
                _logger.LogInformation("Retrieving worker types");

                var types = EnumHelper.GetEnumList<WorkerType>();

                return types;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving worker types: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<ViewWorkerDto>> GetWorkersByFilterAsync(WorkerFilter workerFilter)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Retrieving workers by filter", userContext);

                var workers = await _unitOfWork.Workers.GetWorkersByFilterAsync(
                    workerName: workerFilter.WorkerName,
                    type: workerFilter.Type);

                _logger.LogInformation("{userContext} - Retrieved {Count} workers", userContext, workers.Count());

                return workers.Select(w => w.ToWorkerDto());
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error retrieving workers: {Message}", userContext, ex.Message);
                throw;
            }
        }
    }
}
