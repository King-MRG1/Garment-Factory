using Repository.Interfaces;
using Services.Interfaces;
using Shared.Dtos.FabricDtos;
using Shared.Dtos.QueryFilters;
using Shared.Dtos.ReportsDtos;
using Shared.Interfaces;
using Shared.Mapping;

namespace Services.Implementations
{
    public class FabricService : IFabricService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        public FabricService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        } 

        public async Task<ViewFabricDto?> CreateFabricAsync(CreateFabricDto createFabricDto)
        {
            var fabric = createFabricDto.ToFabric();

            var userId = _currentUserService.GetCurrentUserId();

            if(string.IsNullOrWhiteSpace(userId))
                throw new InvalidOperationException("User ID not found in authentication context.");

            fabric.UserId = userId;

            await _unitOfWork.Fabrics.AddAsync(fabric);
            await _unitOfWork.SaveChangesAsync();

            return await GetFabricByIdAsync(fabric.Id);
        }

        public async Task<ViewFabricDto?> DeleteFabricAsync(int id)
        {
            var fabric = await _unitOfWork.Fabrics.GetByIdAsync(id);

            if(fabric == null)
                return null;

            _unitOfWork.Fabrics.Delete(fabric);
            await _unitOfWork.SaveChangesAsync();

            return await GetFabricByIdAsync(id);
        }

        public async Task<ViewFabricDto?> GetFabricByIdAsync(int id)
        {
            var fabric = await _unitOfWork.Fabrics.GetFabricByIdAsync(id);

            if(fabric == null)
                return null;

            return fabric.ToFabricDto();
        }

        public async Task<FabricReportDto?> GetFabricReportAsync(DateOnly StartDate, DateOnly EndDate)
        {
            var fabrics = await _unitOfWork.Fabrics.GetFabricsByDateRangeAsync(StartDate, EndDate);
            
            if(fabrics == null || !fabrics.Any())
                return null;

            return new FabricReportDto
            {
                StartDate = StartDate,
                EndDate = EndDate,
                TotalMeters = fabrics.Sum(f => f.Metres),
                TotalPrice = fabrics.Sum(f => f.Price),

                Fabrics = fabrics
                .GroupBy(f => f.Fabric_Name)
                .Select(g => new ViewFabricReportDto {
                    FabricName = g.Key,
                    TotalMeters = g.Sum(f => f.Metres),
                    TotalPrice = g.Sum(f => f.Price),
                    Trader_Names = g.Select(f => f.Trader.Trader_Name)
                                    .Distinct()
                                    .ToList()
                }).ToList()
            };
        }

        public async Task<IEnumerable<ViewFabricDto>> GetFabricsByFilterAsync(FabricFilter fabricFillter)
        {
            var fabrics = await _unitOfWork.Fabrics.GetFabricsByFilterAsync(
                fabricName: fabricFillter.FabricName,
                traderName: fabricFillter.TraderName,
                startDate: fabricFillter.StartDate,
                endDate: fabricFillter.EndDate
                );
            
            return fabrics.Select(f => f.ToFabricDto());
        }

        public async Task<ViewFabricDto?> UpdateFabricAsync(int id, UpdateFabricDto updateFabricDto)
        {
            var fabric = await _unitOfWork.Fabrics.GetByIdAsync(id);

            if(fabric == null)
                return null;

            fabric.UpdateFabric(updateFabricDto);
            await _unitOfWork.SaveChangesAsync();

            return await GetFabricByIdAsync(fabric.Id);
        }
    }
}
