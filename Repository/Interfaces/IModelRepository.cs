using Database.Models;

namespace Repository.Interfaces
{
    public interface IModelRepository : IGenericRepository<Model>
    {
        public Task<IEnumerable<Model>> GetAllModelsAsync();
        public Task<Model> GetModelByIdAsync(int id);
        public Task<IEnumerable<Model>> GetModelByNameAsync(string name);
        public Task<IEnumerable<Model>> GetModelsByIdsAsync(List<int> ids);

    }
}
