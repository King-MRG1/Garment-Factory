using Database.Models;

namespace Repository.Interfaces
{
    public interface IAdvanceAndDeductionRepository : IGenericRepository<AdvanceAndDeduction>
    {
        public Task<IEnumerable<AdvanceAndDeduction>> GetAllAdvanceAndDeductionsAsync();
        public Task<AdvanceAndDeduction> GetAdvanceAndDeductionsByIdAsync(int id);
        public Task<IEnumerable<AdvanceAndDeduction>> GetAllAdvanceAndDeductionsByTypeAsync(int type);
    }
}
