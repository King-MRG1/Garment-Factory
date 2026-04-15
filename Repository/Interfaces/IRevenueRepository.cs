using Database.Models;

namespace Repository.Interfaces
{
    public interface IRevenueRepository : IGenericRepository<Revenue>
    {
        public Task<Revenue> GetRevenueByIdAsync(int id);
        public Task<IEnumerable<Revenue>> GetRevenuesByFilterAsync(string? traderName);
    }
}
