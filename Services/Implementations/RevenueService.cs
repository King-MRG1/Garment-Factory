using Database.Models;
using Repository.Interfaces;
using Services.Interfaces;
using Shared.Dtos.ExpenseDtos;
using Shared.Dtos.QueryFilters;
using Shared.Dtos.RevenueDtos;
using Shared.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Implementations
{
    public class RevenueService : IRevenueService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        public RevenueService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ViewRevenueDto?> AddRevenueAsync(CreateRevenueDto createRevenue)
        {
            var revenue = createRevenue.ToRevenue();

            if(revenue.Trader_Id.HasValue)
                await EditAmountToTrader(revenue.Trader_Id.Value, revenue.Amount);
            
            var userId = _currentUserService.GetCurrentUserId();
            if (string.IsNullOrWhiteSpace(userId))
                throw new InvalidOperationException("UserID not found in authentication context.");

            revenue.UserId = userId;

            await _unitOfWork.Revenues.AddAsync(revenue);
            await _unitOfWork.SaveChangesAsync();

            return revenue.ToRevenueDto();
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
            var revenue = await _unitOfWork.Revenues.GetRevenueByIdAsync(id);

            if (revenue == null)
                return null;

            if(revenue.Trader_Id.HasValue)
                await EditAmountToTrader(revenue.Trader_Id, -revenue.Amount);

            _unitOfWork.Revenues.Delete(revenue);
            await _unitOfWork.SaveChangesAsync();

            return revenue.ToRevenueDto();
        }

        public async Task<IEnumerable<ViewRevenueDto>> GetAllRevenuesAsync()
        {
            var revenues = await _unitOfWork.Revenues.GetAllRevenueAsync();

            return revenues.Select(r => r.ToRevenueDto());
        }

        public async Task<ViewRevenueDto?> GetRevenueByIdAsync(int id)
        {
            var revenue = await _unitOfWork.Revenues.GetRevenueByIdAsync(id);

            if (revenue == null)
                return null;

            return revenue.ToRevenueDto();
        }

        public async Task<IEnumerable<ViewRevenueDto>> GetRevenueByTraderIdAsync(int traderId)
        {
            var revenues = await _unitOfWork.Revenues.GetRevenuesByTraderIdAsync(traderId);

            return revenues.Select(r => r.ToRevenueDto());
        }

        public async Task<ViewRevenueDto?> UpdateRevenueAsync(int id,UpdateRevenueDto updateRevenue)
        {
            var revenue = await _unitOfWork.Revenues.GetRevenueByIdAsync(id);

            if (revenue == null)
                return null;

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

            return revenue.ToRevenueDto();
        }

        public async Task<IEnumerable<ViewRevenueDto>> GetRevenuesByFilterAsync(RevenueFilter revenueFilter)
        {
            var revenues = await _unitOfWork.Revenues.GetRevenuesByFilterAsync(traderName: revenueFilter.TraderName);

            return revenues.Select(r => r.ToRevenueDto());
        }
    }
}
