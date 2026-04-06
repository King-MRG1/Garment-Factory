using Database.Models;
using Shared.Dtos.QueryFilters;
using Shared.Dtos.TraderDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface ITraderService
    {
        public Task<ViewTraderDto?> GetTrader(int id);
        public Task<ViewTraderDto?> AddTrader(CreateTraderDto createTraderDto);
        public Task<ViewTraderDto?> UpdateTrader(int id, UpdateTraderDto updateTraderDto);
        public Task<ViewTraderDto?> DeleteTrader(int id);
        public Task<IEnumerable<ViewTraderDto>> GetAllTraders();
        public Task<IEnumerable<ViewTraderDto>> GetTradersByName(string name);
        public Task<IEnumerable<ViewTraderDto>> GetTradersByType(TraderType traderType);
        public Task<IEnumerable<ViewTraderDto>> GetTradersByFilter(TraderFilter traderFilter);
        public IEnumerable<object> GetTraderTypes();
    }
}
