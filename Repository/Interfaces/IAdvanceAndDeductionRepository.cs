using Database.Models;

namespace Repository.Interfaces
{
    public interface IAdvanceAndDeductionRepository : IGenericRepository<AdvanceAndDeduction>
    {
        public Task<AdvanceAndDeduction> GetAdvanceAndDeductionsByIdAsync(int id);
        public Task<IEnumerable<AdvanceAndDeduction>> GetAdvancesAndDeductionsByFilterAsync
            (int? type, DateOnly? startDate, DateOnly? endDate, string? workerName,bool? isUsed);
        public Task MakeAdvanceAndDeductionUsed(List<int> advanceAndDeductionIds);
    }
}
