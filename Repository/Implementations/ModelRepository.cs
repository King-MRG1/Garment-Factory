using Database.Data;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Repository.Interfaces;

namespace Repository.Implementations
{
    public class ModelRepository : GenericRepository<Model>, IModelRepository
    {
        public ModelRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Model>> GetAllModelsAsync()
        {
            var models = await _context.Models
                .Include(m => m.OrderModels)
                .ThenInclude(om => om.Order)
                .ToListAsync();

            return models;
        }

        public async Task<Model> GetModelByIdAsync(int id)
        {
            var model = await _context.Models
                .Include(m => m.OrderModels)
                .ThenInclude(om => om.Order)
                .FirstOrDefaultAsync(m => m.Id == id);

            return model;
        }

        public async Task<IEnumerable<Model>> GetModelByNameAsync(string name)
        {
            var models = await _context.Models
                 .Include(m => m.OrderModels)
                 .ThenInclude(om => om.Order)
                 .Where(m => m.Model_Name.Contains(name))
                 .ToListAsync();

            return models;
        }

        public async Task<IEnumerable<Model>> GetModelsByIdsAsync(List<int> ids)
        {
            var models = await _context.Models
                .Where(m => ids.Contains(m.Id))
                .ToListAsync();
            
            return models;
        }
    }
}
