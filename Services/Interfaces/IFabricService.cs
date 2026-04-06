using Shared.Dtos.FabricDtos;
using Shared.Dtos.QueryFilters;
using Shared.Dtos.ReportsDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IFabricService
    {
        public Task<ViewFabricDto?> GetFabricByIdAsync(int id);
        public Task<IEnumerable<ViewFabricDto>> GetAllFabricsAsync();
        public Task<ViewFabricDto?> CreateFabricAsync(CreateFabricDto createFabricDto);
        public Task<ViewFabricDto?> UpdateFabricAsync(int id, UpdateFabricDto updateFabricDto);
        public Task<ViewFabricDto?> DeleteFabricAsync(int id);
        public Task<FabricReportDto?> GetFabricReportAsync(DateOnly StartDate , DateOnly EndDate);
        public Task<IEnumerable<ViewFabricDto>> GetFabricsByTraderNameAsync(string traderName);
        public Task<IEnumerable<ViewFabricDto>> GetFabricsByNameAsync(string name);
        public Task<IEnumerable<ViewFabricDto>> GetFabricsByFilterAsync(FabricFilter fabricFilter);

    }
}
