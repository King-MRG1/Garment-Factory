using Database.Models;
using Shared.Dtos.RevenueDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Mapping
{
    public static class RevenueMapping
    {
        public static ViewRevenueDto ToRevenueDto(this Revenue revenue)
        {
            return new ViewRevenueDto
            {
                Id = revenue.Id,
                Revenue_Name = revenue.Revenue_Name,
                Revenue_Description = revenue.Revenue_Description,
                Amount = revenue.Amount,
                Revenue_Date = revenue.Revenue_Date,
                Last_Update = revenue.Last_Update,
                Trader_Name = revenue.Trader != null ? revenue.Trader.Trader_Name : null
            };
        }
        public static Revenue ToRevenue(this CreateRevenueDto createRevenueDto)
        {
            return new Revenue
            {
                Revenue_Name = createRevenueDto.Revenue_Name,
                Revenue_Description = createRevenueDto.Revenue_Description,
                Amount = createRevenueDto.Amount,
                Revenue_Date = DateOnly.FromDateTime(DateTime.Now),
                Last_Update = DateOnly.FromDateTime(DateTime.Now),
                Trader_Id = createRevenueDto.Trader_Id
            };
        }
        public static void UpdateRevenue(this Revenue revenue, UpdateRevenueDto updateRevenueDto)
        {
            revenue.Revenue_Name = updateRevenueDto.Revenue_Name;
            revenue.Revenue_Description = updateRevenueDto.Revenue_Description;
            revenue.Amount = updateRevenueDto.Amount;
            revenue.Trader_Id = updateRevenueDto.Trader_Id;
            revenue.Last_Update = DateOnly.FromDateTime(DateTime.Now);
        }
    }
}
