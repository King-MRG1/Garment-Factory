using Microsoft.Extensions.Logging;
using Repository.Interfaces;
using Services.Interfaces;
using Shared.Dtos.FabricDtos;
using Shared.Dtos.QueryFilters;
using Shared.Dtos.ReportsDtos;
using Shared.Helper;
using Shared.Interfaces;
using Shared.Mapping;

namespace Services.Implementations
{
    public class FabricService : IFabricService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<FabricService> _logger;

        public FabricService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, ILogger<FabricService> logger)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _logger = logger;
        } 

        public async Task<ViewFabricDto?> CreateFabricAsync(CreateFabricDto createFabricDto)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Creating new fabric", userContext);

                var fabric = createFabricDto.ToFabric();

                if (fabric == null)
                    throw new InvalidOperationException("Failed to map fabric DTO to entity.");

                var userId = _currentUserService.GetCurrentUserId();

                if (string.IsNullOrWhiteSpace(userId))
                    throw new InvalidOperationException("User ID not found in authentication context.");

                fabric.UserId = userId;

                await _unitOfWork.Fabrics.AddAsync(fabric);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("{userContext} - Fabric created successfully. Fabric ID: {Id}", userContext, fabric.Id);

                return await GetFabricByIdAsync(fabric.Id);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("{userContext} - Validation error: {Message}", userContext, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error creating fabric: {Message}", userContext, ex.Message);
                throw;
            }
        }

        public async Task<ViewFabricDto?> DeleteFabricAsync(int id)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Deleting fabric {Id}", userContext, id);

                var fabric = await _unitOfWork.Fabrics.GetByIdAsync(id);

                if (fabric == null)
                {
                    _logger.LogWarning("{userContext} - Fabric {Id} not found", userContext, id);
                    return null;
                }

                _unitOfWork.Fabrics.Delete(fabric);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("{userContext} - Fabric {Id} deleted successfully", userContext, id);

                return await GetFabricByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error deleting fabric: {Message}", userContext, ex.Message);
                throw;
            }
        }

        public async Task<ViewFabricDto?> GetFabricByIdAsync(int id)
        {
            try
            {
                var fabric = await _unitOfWork.Fabrics.GetFabricByIdAsync(id);

                if (fabric == null)
                    return null;

                return fabric.ToFabricDto();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving fabric {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<FabricReportDto?> GetFabricReportAsync(DateOnly StartDate, DateOnly EndDate)
        {
            try
            {
                _logger.LogInformation("Retrieving fabric report from {StartDate} to {EndDate}", StartDate, EndDate);

                var fabrics = await _unitOfWork.Fabrics.GetFabricsByDateRangeAsync(StartDate, EndDate);

                if (fabrics == null || !fabrics.Any())
                {
                    _logger.LogWarning("No fabrics found for date range {StartDate} to {EndDate}", StartDate, EndDate);
                    return null;
                }

                var groupedFabrics = fabrics
                 .GroupBy(f => f.Fabric_Name)
                 .Select(g => new ViewFabricReportDto
                 {
                     FabricName = g.Key,
                     TotalMeters = g.Sum(f => f.Metres),
                     TotalPrice = g.Sum(f => f.Price),
                     Trader_Names = g.Select(f => f.Trader.Trader_Name)
                                     .Distinct()
                                     .ToList()
                 })
                 .ToList();

                _logger.LogInformation("Fabric report generated successfully with {Count} fabrics", groupedFabrics.Count);

                return new FabricReportDto
                {
                    StartDate = StartDate,
                    EndDate = EndDate,
                    TotalMeters = fabrics.Sum(f => f.Metres),
                    TotalPrice = fabrics.Sum(f => f.Price),
                    Fabrics = groupedFabrics
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving fabric report: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<ViewFabricDto>> GetFabricsByFilterAsync(FabricFilter fabricFillter)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Retrieving fabrics by filter", userContext);

                var fabrics = await _unitOfWork.Fabrics.GetFabricsByFilterAsync(
                    fabricName: fabricFillter.FabricName,
                    traderName: fabricFillter.TraderName,
                    startDate: fabricFillter.StartDate,
                    endDate: fabricFillter.EndDate);

                _logger.LogInformation("{userContext} - Retrieved {Count} fabrics", userContext, fabrics.Count());

                return fabrics.Select(f => f.ToFabricDto());
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error retrieving fabrics: {Message}", userContext, ex.Message);
                throw;
            }
        }

        public async Task<ViewFabricDto?> UpdateFabricAsync(int id, UpdateFabricDto updateFabricDto)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Updating fabric {Id}", userContext, id);

                var fabric = await _unitOfWork.Fabrics.GetByIdAsync(id);

                if (fabric == null)
                {
                    _logger.LogWarning("{userContext} - Fabric {Id} not found", userContext, id);
                    return null;
                }

                fabric.UpdateFabric(updateFabricDto);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("{userContext} - Fabric {Id} updated successfully", userContext, id);

                return await GetFabricByIdAsync(fabric.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error updating fabric: {Message}", userContext, ex.Message);
                throw;
            }
        }
    }
}
