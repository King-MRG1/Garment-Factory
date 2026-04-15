using Database.Models;

namespace Repository.Interfaces
{
    public interface IWorkerRepository : IGenericRepository<Worker>
    {
        public Task<Worker> GetWorkerByIdAsync(int id);
        public Task<IEnumerable<Worker>> GetWorkersByFilterAsync(string? workerName, int? type);
    }
}
