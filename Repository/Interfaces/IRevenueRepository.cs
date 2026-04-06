using Database.Models;

namespace Repository.Interfaces
{
    public interface IRevenueRepository : IGenericRepository<Revenue>
    {
        public Task<IEnumerable<Revenue>> GetAllRevenueAsync();
        public Task<IEnumerable<Revenue>> GetRevenuesByTraderIdAsync(int traderId);
        public Task<Revenue> GetRevenueByIdAsync(int id);
        public Task<IEnumerable<Revenue>> GetRevenuesByFilterAsync(string? traderName);


    }
}
