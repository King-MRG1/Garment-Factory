using Database.Models;

namespace Repository.Interfaces
{
    public interface IFabricRepository : IGenericRepository<Fabric>
    {
        public Task<IEnumerable<Fabric>> GetAllFabricsWithTradersAsync();
        public Task<Fabric> GetFabricWithTraderByIdAsync(int id);
        public Task<IEnumerable<Fabric>> GetFabricsByTraderIdAsync(int traderId);
        public Task<IEnumerable<Fabric>> GetFabricsByNameAsync(string name);
        public Task<IEnumerable<Fabric>> GetFabricsByDateRangeAsync(DateOnly startDate, DateOnly endDate);
    }
}
