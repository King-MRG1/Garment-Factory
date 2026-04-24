using Shared;
using Shared.Dtos;
using Shared.Dtos.AdvanceAndDeductionDtos;
using Shared.Dtos.QueryFilters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IAdvanceAndDeductionService
    {
        public Task<IEnumerable<ViewEnumDto>> GetTypesAsync();

        public Task<IEnumerable<ViewAdvanceAndDeductionDto>>
            GetAdvancesAndDeductionsByFilterAsync(AdvanceAndDeductionFilter filter);
        public Task<ViewAdvanceAndDeductionDto> GetAdvanceOrDeductionByIdAsync(int id);
        public Task<ViewAdvanceAndDeductionDto> CreateAdvanceOrDeductionAsync(CreateAdvanceAndDeductionDto dto);
        public Task<ViewAdvanceAndDeductionDto> UpdateAdvanceOrDeductionAsync(int id, UpdateAdvanceAndDeductionDto dto);
        public Task<ViewAdvanceAndDeductionDto> DeleteAdvanceOrDeductionAsync(int id);
    }
}
