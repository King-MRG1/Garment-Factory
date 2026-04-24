using Database.Models;
using Shared.Dtos.ExpenseDtos;
using Shared.Dtos.FabricDtos;
using Shared.Dtos.OrderDtos;
using Shared.Dtos.PhoneDtos;
using Shared.Dtos.RevenueDtos;
using Shared.Dtos.TraderDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Mapping
{
    public static class TraderMapping
    {
        public static ViewTraderDto ToTraderDto(this Trader trader)
        {
            return new ViewTraderDto
            {
                Id = trader.Id,
                Trader_Name = trader.Trader_Name,
                Address = trader.Address,
                Trader_Type = trader.Trader_Type.ToString(),
                Amount = trader.Amount,
                Register_date = trader.Register_date,
                Last_Update = trader.Last_Update,
                Phones = SafeMapping(trader.Phones, p => p.ToPhoneDto()),
                Fabrics = SafeMapping(trader.Fabrics, f => f.ToFabricDto()),
                Payments = SafeMapping(trader.Revenues, r => r.ToRevenueDto()),
                Purchases = SafeMapping(trader.Expenses, e => e.ToExpenseDto()),
                Orders = SafeMapping(trader.Orders, o => o.ToOrderDto())
            };
        }
        public static Trader ToTrader(this CreateTraderDto createTraderDto)
        {
            return new Trader
            {
                Trader_Name = createTraderDto.Trader_Name,
                Address = createTraderDto.Address,
                Phones = SafeMapping(createTraderDto.Phones, p => p.ToPhone()),
                Trader_Type = (TraderType)createTraderDto.Trader_Type,
                Amount = createTraderDto.Amount,
                Register_date = DateOnly.FromDateTime(DateTime.Now),
                Last_Update = DateOnly.FromDateTime(DateTime.Now)
            };
        }
        public static void UpdateTrader(this Trader trader, UpdateTraderDto updateTraderDto)
        {
            trader.Trader_Name = updateTraderDto.Trader_Name;
            trader.Address = updateTraderDto.Address;
            trader.Trader_Type = (TraderType)updateTraderDto.Trader_Type;
            trader.Amount = updateTraderDto.Amount;
            trader.Last_Update = DateOnly.FromDateTime(DateTime.Now);
        }
        public static List<TDto> SafeMapping<TEntity, TDto>(
            IEnumerable<TEntity> entities,
            Func<TEntity,TDto> mapper)
        {
            return entities?.Select(mapper).ToList() ?? new List<TDto>();
        }
    }
}
