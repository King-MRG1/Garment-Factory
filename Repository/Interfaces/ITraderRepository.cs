using Database.Models;

namespace Repository.Interfaces
{
    public interface ITraderRepository : IGenericRepository<Trader>
    {
        public Task<IEnumerable<Trader>> GetTradersAsync();
        public Task<Trader> GetTraderByIdAsync(int id);
        public Task<IEnumerable<Trader>> GetTraderByNameAsync(string name);
        public Task<IEnumerable<Trader>> GetTraderByTypeAsync(TraderType type);
    }
}
