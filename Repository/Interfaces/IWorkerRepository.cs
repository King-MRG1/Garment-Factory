using Database.Models;

namespace Repository.Interfaces
{
    public interface IWorkerRepository : IGenericRepository<Worker>
    {
        public Task<IEnumerable<Worker>> GetWorkersAsync();
        public Task<Worker> GetWorkerByIdAsync(int id);
        public Task<IEnumerable<Worker>> GetWorkersByNameAsync(string name);
        public Task<IEnumerable<Worker>> GetWorkersByTypeAsync(WorkerType type);
        public Task<IEnumerable<Worker>> GetWorkersByFilterAsync(string? workerName, int? type);
    }
}
