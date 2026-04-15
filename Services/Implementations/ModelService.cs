using Services.Interfaces;
using Shared.Dtos.ModelDtos;
using Shared.Mapping;
using Repository.Interfaces;
using Shared.Dtos.QueryFilters;
using Shared.Interfaces;

namespace Services.Implementations
{
    public class ModelService : IModelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public ModelService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ViewModelDto?> CreateModelAsync(CreateModelDto createModelDto)
        {
              var model = createModelDto.ToModel();

              var userId = _currentUserService.GetCurrentUserId();

            if(string.IsNullOrWhiteSpace(userId)) 
                throw new InvalidOperationException("UserID not found in authentication context.");

            model.UserId = userId;

            await _unitOfWork.Models.AddAsync(model);
            await _unitOfWork.SaveChangesAsync();

            return model.ToModelDto();
        }

        public async Task<ViewModelDto?> AddQuantityToModelAsync(int id, int quantityToAdd)
        {
            var model = await _unitOfWork.Models.GetByIdAsync(id);

            if (model == null)
                return null;

            model.Total_Units += quantityToAdd;

            await _unitOfWork.SaveChangesAsync();

            return model.ToModelDto();
        }

        public async Task<ViewModelDto?> DeleteModelAsync(int id)
        {
            var model = await _unitOfWork.Models.GetByIdAsync(id);

            if (model == null)
                return null;

            _unitOfWork.Models.Delete(model);
            await _unitOfWork.SaveChangesAsync();

            return model.ToModelDto();
        }

        public async Task<ViewModelDto?> GetModelByIdAsync(int id)
        {
            var model = await _unitOfWork.Models.GetByIdAsync(id);

            if (model == null)
                return null;
            

            return model.ToModelDto();
        }

        public async Task<ViewModelDto?> UpdateModelAsync(int id, UpdateModelDto updateModelDto)
        {
            var model = await _unitOfWork.Models.GetByIdAsync(id);

            if(model == null)
                return null;

            model.UpdateModel(updateModelDto);

            await _unitOfWork.SaveChangesAsync();

            return model.ToModelDto();
        }

        public async Task<IEnumerable<ViewModelDto>> GetModelsByFilterAsync(ModelFilter modelFilter)
        {
            var models = await _unitOfWork.Models.GetModelsByFilterAsync(modelName: modelFilter.ModelName);

            return models.Select(m => m.ToModelDto());
        }
    }
}
