using Shared.Dtos.ModelDtos;
using Shared.Dtos.QueryFilters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IModelService
    {
        public Task<ViewModelDto?> GetModelByIdAsync(int id);
        public Task<IEnumerable<ViewModelDto>> GetAllModelsAsync();
        public Task<ViewModelDto?> CreateModelAsync(CreateModelDto createModelDto);
        public Task<ViewModelDto?> UpdateModelAsync(int id, UpdateModelDto updateModelDto);
        public Task<ViewModelDto?> DeleteModelAsync(int id);
        public Task<ViewModelDto?> AddQuantityToModelAsync(int id, int quantityToAdd);
        public Task<IEnumerable<ViewModelDto>> GetModelsByFilterAsync(ModelFilter modelFilter);
    }
}
