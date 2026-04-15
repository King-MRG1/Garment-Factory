using Shared.Dtos.QueryFilters;
using Shared.Dtos.RevenueDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IRevenueService
    {
        public Task<ViewRevenueDto?> GetRevenueByIdAsync(int id);
        public Task<IEnumerable<ViewRevenueDto>> GetRevenuesByFilterAsync(RevenueFilter revenueFilter);
        public Task<ViewRevenueDto?> AddRevenueAsync(CreateRevenueDto createRevenue);
        public Task<ViewRevenueDto?> UpdateRevenueAsync(int id,UpdateRevenueDto updateRevenue);
        public Task<ViewRevenueDto?> DeleteRevenue(int id);
    }
}
