using Database.Models;
using Repository.Interfaces;
using Services.Interfaces;
using Shared.Interfaces;
using Shared.Dtos;
using Shared.Dtos.AdvanceAndDeductionDtos;
using Shared.Dtos.QueryFilters;
using Shared.Helper;
using Shared.Mapping;

namespace Services.Implementations
{
    public class AdvanceAndDeductionService : IAdvanceAndDeductionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        public AdvanceAndDeductionService(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<ViewEnumDto>> GetTypesAsync()
        {
            var types = EnumHelper.GetEnumList<AdvanceOrDeduction>();

            return types;
        }

        public async Task<IEnumerable<ViewAdvanceAndDeductionDto>>
            GetAdvancesAndDeductionsByFilterAsync(AdvanceAndDeductionFilter filter)
        {
            var advancesAndDeductions = await _unitOfWork.AdvanceAndDeductions
                .GetAdvancesAndDeductionsByFilterAsync(
               type: filter.Type ?? 0,
                startDate: filter.StartDate,
                endDate: filter.EndDate,
                workerName: filter.WorkerName);

            return advancesAndDeductions.Select(a => a.ToAdvanceAndDeductionDto());
        }

        public async Task<ViewAdvanceAndDeductionDto> CreateAdvanceOrDeductionAsync(CreateAdvanceAndDeductionDto dto)
        {
            var advanceAndDeduction = dto.ToAdvanceAndDeduction();
            var userId = _currentUserService.GetCurrentUserId();

            if(userId == null)
                throw new InvalidOperationException("UserID not found in authentication context.");

            advanceAndDeduction.UserId = userId;

            await _unitOfWork.AdvanceAndDeductions.AddAsync(advanceAndDeduction);
            await _unitOfWork.SaveChangesAsync();

            return advanceAndDeduction.ToAdvanceAndDeductionDto();
        }

        public async Task<ViewAdvanceAndDeductionDto> UpdateAdvanceOrDeductionAsync(int id, UpdateAdvanceAndDeductionDto dto)
        {
            var existing = await _unitOfWork.AdvanceAndDeductions.GetByIdAsync(id);

            if (existing == null)
                throw new KeyNotFoundException($"Advance or Deduction with ID {id} not found.");

            existing.UpdateAdvanceAndDeduction(dto);

            _unitOfWork.AdvanceAndDeductions.Update(existing);
            await _unitOfWork.SaveChangesAsync();
            return existing.ToAdvanceAndDeductionDto();
        }

        public async Task<ViewAdvanceAndDeductionDto> DeleteAdvanceOrDeductionAsync(int id)
        {
            var existing = await _unitOfWork.AdvanceAndDeductions.GetByIdAsync(id);

            if (existing == null)
                return null;

            _unitOfWork.AdvanceAndDeductions.Delete(existing);
            await _unitOfWork.SaveChangesAsync();

            return existing.ToAdvanceAndDeductionDto();
        }

        public async Task<ViewAdvanceAndDeductionDto> GetAdvanceOrDeductionByIdAsync(int id)
        {
            var existing = await _unitOfWork.AdvanceAndDeductions.GetAdvanceAndDeductionsByIdAsync(id);

            if (existing == null)
                return null;

            return existing.ToAdvanceAndDeductionDto();
        }
    }
}
