using Database.Models;

namespace Repository.Interfaces
{
    public interface ITraderRepository : IGenericRepository<Trader>
    {
        public Task<Trader> GetTraderByIdAsync(int? id);
        public Task<IEnumerable<Trader>> GetTradersByFilterAsync(string? traderName, int? type);

    }
}
