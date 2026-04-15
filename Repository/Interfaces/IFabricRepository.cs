using Database.Models;

namespace Repository.Interfaces
{
    public interface IFabricRepository : IGenericRepository<Fabric>
    {
        public Task<Fabric?> GetFabricByIdAsync(int id);
        public Task<IEnumerable<Fabric>> GetFabricsByDateRangeAsync(DateOnly startDate, DateOnly endDate);
        public Task<IEnumerable<Fabric>> GetFabricsByFilterAsync(string fabricName, string traderName, DateOnly? startDate, DateOnly? endDate);
    }
}
