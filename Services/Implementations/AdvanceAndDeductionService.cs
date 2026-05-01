using Database.Models;
using Microsoft.Extensions.Logging;
using Repository.Interfaces;
using Services.Interfaces;
using Shared.Interfaces;
using Shared.Dtos;
using Shared.Dtos.AdvanceAndDeductionDtos;
using Shared.Dtos.QueryFilters;
using Shared.Helper;
using Shared.Mapping;
using Shared;

namespace Services.Implementations
{
    public class AdvanceAndDeductionService : IAdvanceAndDeductionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<AdvanceAndDeductionService> _logger;

        public AdvanceAndDeductionService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, ILogger<AdvanceAndDeductionService> logger)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<IEnumerable<ViewEnumDto>> GetTypesAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving advance and deduction types");

                var types = EnumHelper.GetEnumList<AdvanceOrDeduction>();

                return types;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving types: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<ViewAdvanceAndDeductionDto>>
            GetAdvancesAndDeductionsByFilterAsync(AdvanceAndDeductionFilter filter)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Retrieving advances and deductions by filter", userContext);

                var advancesAndDeductions = await _unitOfWork.AdvanceAndDeductions
                    .GetAdvancesAndDeductionsByFilterAsync(
                   type: filter.Type ?? 0,
                    startDate: filter.StartDate,
                    endDate: filter.EndDate,
                    workerName: filter.WorkerName,
                    isUsed: filter.IsUsed);

                _logger.LogInformation("{userContext} - Retrieved {Count} records", userContext, advancesAndDeductions.Count());

                return advancesAndDeductions.Select(a => a.ToAdvanceAndDeductionDto());
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error retrieving advances and deductions: {Message}", userContext, ex.Message);
                throw;
            }
        }

        public async Task<ViewAdvanceAndDeductionDto> CreateAdvanceOrDeductionAsync(
            CreateAdvanceAndDeductionDto dto)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Creating new advance or deduction", userContext);

                var advanceAndDeduction = dto.ToAdvanceAndDeduction();

                if (advanceAndDeduction == null)
                    throw new InvalidOperationException("Failed to map advance/deduction DTO to entity.");

                var userId = _currentUserService.GetCurrentUserId();

                if (string.IsNullOrWhiteSpace(userId))
                    throw new InvalidOperationException("UserID not found in authentication context.");

                advanceAndDeduction.UserId = userId;

                await _unitOfWork.AdvanceAndDeductions.AddAsync(advanceAndDeduction);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("{userContext} - Advance/Deduction created successfully. ID: {Id}", userContext, advanceAndDeduction.Id);

                return await GetAdvanceOrDeductionByIdAsync(advanceAndDeduction.Id);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("{userContext} - Validation error: {Message}", userContext, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error creating advance/deduction: {Message}", userContext, ex.Message);
                throw;
            }
        }

        public async Task<ViewAdvanceAndDeductionDto> UpdateAdvanceOrDeductionAsync(
            int id, UpdateAdvanceAndDeductionDto dto)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Updating advance/deduction {Id}", userContext, id);

                var existing = await _unitOfWork.AdvanceAndDeductions.GetByIdAsync(id);

                if (existing == null)
                    throw new KeyNotFoundException($"Advance or Deduction with ID {id} not found.");

                existing.UpdateAdvanceAndDeduction(dto);

                _unitOfWork.AdvanceAndDeductions.Update(existing);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("{userContext} - Advance/Deduction {Id} updated successfully", userContext, id);

                return await GetAdvanceOrDeductionByIdAsync(existing.Id);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("{userContext} - Record not found: {Message}", userContext, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error updating advance/deduction: {Message}", userContext, ex.Message);
                throw;
            }
        }

        public async Task<ViewAdvanceAndDeductionDto> DeleteAdvanceOrDeductionAsync(int id)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Deleting advance/deduction {Id}", userContext, id);

                var existing = await _unitOfWork.AdvanceAndDeductions.GetByIdAsync(id);

                if (existing == null)
                {
                    _logger.LogWarning("{userContext} - Advance/Deduction {Id} not found", userContext, id);
                    return null;
                }

                _unitOfWork.AdvanceAndDeductions.Delete(existing);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("{userContext} - Advance/Deduction {Id} deleted successfully", userContext, id);

                return existing.ToAdvanceAndDeductionDto();
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error deleting advance/deduction: {Message}", userContext, ex.Message);
                throw;
            }
        }

        public async Task<ViewAdvanceAndDeductionDto> GetAdvanceOrDeductionByIdAsync(int id)
        {
            try
            {
                var existing = await _unitOfWork.AdvanceAndDeductions.GetAdvanceAndDeductionsByIdAsync(id);

                if (existing == null)
                    return null;

                return existing.ToAdvanceAndDeductionDto();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving advance/deduction {Id}: {Message}", id, ex.Message);
                throw;
            }
        }
    }
}
