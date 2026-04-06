using Database.Models;

namespace Repository.Interfaces
{
    public interface IModelRepository : IGenericRepository<Model>
    {
        public Task<IEnumerable<Model>> GetAllModelsAsync();
        public Task<Model> GetModelByIdAsync(int id);
        public Task<IEnumerable<Model>> GetModelsByIdsAsync(List<int> ids);
        public Task<IEnumerable<Model>> GetModelsByFilterAsync(string? modelName);

    }
}
