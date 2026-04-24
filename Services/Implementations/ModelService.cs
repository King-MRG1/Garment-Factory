using Microsoft.Extensions.Logging;
using Repository.Interfaces;
using Services.Interfaces;
using Shared.Dtos.ModelDtos;
using Shared.Dtos.QueryFilters;
using Shared.Helper;
using Shared.Interfaces;
using Shared.Mapping;

namespace Services.Implementations
{
    public class ModelService : IModelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<ModelService> _logger;

        public ModelService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, ILogger<ModelService> logger)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<ViewModelDto?> CreateModelAsync(CreateModelDto createModelDto)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Creating new model", userContext);

                var model = createModelDto.ToModel();

                if (model == null)
                    throw new InvalidOperationException("Failed to map model DTO to entity.");

                var userId = _currentUserService.GetCurrentUserId();

                if (string.IsNullOrWhiteSpace(userId))
                    throw new InvalidOperationException("UserID not found in authentication context.");

                model.UserId = userId;

                await _unitOfWork.Models.AddAsync(model);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("{userContext} - Model created successfully. Model ID: {Id}", userContext, model.Id);

                return await GetModelByIdAsync(model.Id);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("{userContext} - Validation error: {Message}", userContext, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error creating model: {Message}", userContext, ex.Message);
                throw;
            }
        }

        public async Task<ViewModelDto?> AddQuantityToModelAsync(int id, int quantityToAdd)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Adding {Quantity} units to model {Id}", userContext, quantityToAdd, id);

                var model = await _unitOfWork.Models.GetByIdAsync(id);

                if (model == null)
                {
                    _logger.LogWarning("{userContext} - Model {Id} not found", userContext, id);
                    return null;
                }

                model.Total_Units += quantityToAdd;

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("{userContext} - Units added to model {Id} successfully", userContext, id);

                return model.ToModelDto();
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error adding units to model: {Message}", userContext, ex.Message);
                throw;
            }
        }

        public async Task<ViewModelDto?> DeleteModelAsync(int id)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Deleting model {Id}", userContext, id);

                var model = await _unitOfWork.Models.GetByIdAsync(id);

                if (model == null)
                {
                    _logger.LogWarning("{userContext} - Model {Id} not found", userContext, id);
                    return null;
                }

                _unitOfWork.Models.Delete(model);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("{userContext} - Model {Id} deleted successfully", userContext, id);

                return model.ToModelDto();
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error deleting model: {Message}", userContext, ex.Message);
                throw;
            }
        }

        public async Task<ViewModelDto?> GetModelByIdAsync(int id)
        {
            try
            {
                var model = await _unitOfWork.Models.GetByIdAsync(id);

                if (model == null)
                    return null;

                return model.ToModelDto();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving model {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<ViewModelDto?> UpdateModelAsync(int id, UpdateModelDto updateModelDto)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Updating model {Id}", userContext, id);

                var model = await _unitOfWork.Models.GetByIdAsync(id);

                if (model == null)
                {
                    _logger.LogWarning("{userContext} - Model {Id} not found", userContext, id);
                    return null;
                }

                model.UpdateModel(updateModelDto);

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("{userContext} - Model {Id} updated successfully", userContext, id);

                return await GetModelByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error updating model: {Message}", userContext, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<ViewModelDto>> GetModelsByFilterAsync(ModelFilter modelFilter)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Retrieving models by filter", userContext);

                var models = await _unitOfWork.Models.GetModelsByFilterAsync(modelName: modelFilter.ModelName);

                _logger.LogInformation("{userContext} - Retrieved {Count} models", userContext, models.Count());

                return models.Select(m => m.ToModelDto());
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error retrieving models: {Message}", userContext, ex.Message);
                throw;
            }
        }
    }
}
