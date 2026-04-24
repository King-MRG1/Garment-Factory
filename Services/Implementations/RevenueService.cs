using Database.Models;
using Microsoft.Extensions.Logging;
using Repository.Interfaces;
using Services.Interfaces;
using Shared.Dtos.ExpenseDtos;
using Shared.Dtos.QueryFilters;
using Shared.Dtos.RevenueDtos;
using Shared.Helper;
using Shared.Interfaces;
using Shared.Mapping;

namespace Services.Implementations
{
    public class RevenueService : IRevenueService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<RevenueService> _logger;

        public RevenueService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, ILogger<RevenueService> logger)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<ViewRevenueDto?> AddRevenueAsync(CreateRevenueDto createRevenue)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Creating new revenue", userContext);

                var revenue = createRevenue.ToRevenue();

                if (revenue == null)
                    throw new InvalidOperationException("Failed to map revenue DTO to entity.");

                if (revenue.Trader_Id.HasValue)
                    await EditAmountToTrader(revenue.Trader_Id.Value, revenue.Amount);

                var userId = _currentUserService.GetCurrentUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    throw new InvalidOperationException("UserID not found in authentication context.");

                revenue.UserId = userId;

                await _unitOfWork.Revenues.AddAsync(revenue);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("{userContext} - Revenue created successfully. Revenue ID: {Id}", userContext, revenue.Id);

                return await GetRevenueByIdAsync(revenue.Id);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("{userContext} - Validation error: {Message}", userContext, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error creating revenue: {Message}", userContext, ex.Message);
                throw;
            }
        }

        private async Task EditAmountToTrader(int? traderId, decimal amount)
        {
            var trader = await _unitOfWork.Traders.GetTraderByIdAsync(traderId);

            if (trader == null)
                return;

            if (trader.Trader_Type == TraderType.Customer)
                trader.Amount -= amount;

            else if (trader.Trader_Type == TraderType.Supplier)
                trader.Amount += amount;
        }

        public async Task<ViewRevenueDto?> DeleteRevenue(int id)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Deleting revenue {Id}", userContext, id);

                var revenue = await _unitOfWork.Revenues.GetRevenueByIdAsync(id);

                if (revenue == null)
                {
                    _logger.LogWarning("{userContext} - Revenue {Id} not found", userContext, id);
                    return null;
                }

                if (revenue.Trader_Id.HasValue)
                    await EditAmountToTrader(revenue.Trader_Id, -revenue.Amount);

                _unitOfWork.Revenues.Delete(revenue);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("{userContext} - Revenue {Id} deleted successfully", userContext, id);

                return await GetRevenueByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error deleting revenue: {Message}", userContext, ex.Message);
                throw;
            }
        }

        public async Task<ViewRevenueDto?> GetRevenueByIdAsync(int id)
        {
            try
            {
                var revenue = await _unitOfWork.Revenues.GetRevenueByIdAsync(id);

                if (revenue == null)
                    return null;

                return revenue.ToRevenueDto();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving revenue {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<ViewRevenueDto?> UpdateRevenueAsync(int id, UpdateRevenueDto updateRevenue)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Updating revenue {Id}", userContext, id);

                var revenue = await _unitOfWork.Revenues.GetRevenueByIdAsync(id);

                if (revenue == null)
                {
                    _logger.LogWarning("{userContext} - Revenue {Id} not found", userContext, id);
                    return null;
                }

                if (updateRevenue.Trader_Id.HasValue && updateRevenue.Trader_Id != revenue.Trader_Id)
                {
                    await EditAmountToTrader(revenue.Trader_Id, -revenue.Amount);

                    await EditAmountToTrader(updateRevenue.Trader_Id, updateRevenue.Amount);
                }
                else if (updateRevenue.Trader_Id.HasValue)
                {
                    await EditAmountToTrader(revenue.Trader_Id, updateRevenue.Amount - revenue.Amount);
                }

                revenue.UpdateRevenue(updateRevenue);

                 await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("{userContext} - Revenue {Id} updated successfully", userContext, id);

                return await GetRevenueByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error updating revenue: {Message}", userContext, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<ViewRevenueDto>> GetRevenuesByFilterAsync(RevenueFilter revenueFilter)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Retrieving revenues by filter", userContext);

                var revenues = await _unitOfWork.Revenues.GetRevenuesByFilterAsync(traderName: revenueFilter.TraderName);

                _logger.LogInformation("{userContext} - Retrieved {Count} revenues", userContext, revenues.Count());

                return revenues.Select(r => r.ToRevenueDto());
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error retrieving revenues: {Message}", userContext, ex.Message);
                throw;
            }
        }
    }
}
