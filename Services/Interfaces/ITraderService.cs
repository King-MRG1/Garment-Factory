using Database.Models;
using Shared.Dtos;
using Shared.Dtos.QueryFilters;
using Shared.Dtos.TraderDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface ITraderService
    {
        public Task<ViewTraderDto?> GetTraderByIdAsync(int id);
        public Task<ViewTraderDto?> AddTraderAsync(CreateTraderDto createTraderDto);
        public Task<ViewTraderDto?> UpdateTraderAsync(int id, UpdateTraderDto updateTraderDto);
        public Task<ViewTraderDto?> DeleteTraderAsync(int id);
        public Task<IEnumerable<ViewTraderDto>> GetAllTradersAsync();
        public Task<IEnumerable<ViewTraderDto>> GetTradersByNameAsync(string name);
        public Task<IEnumerable<ViewTraderDto>> GetTradersByTypeAsync(TraderType traderType);
        public Task<IEnumerable<ViewTraderDto>> GetTradersByFilterAsync(TraderFilter traderFilter);
        public Task<IEnumerable<ViewEnumDto>> GetTraderTypesAsync();
    }
}
