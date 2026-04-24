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

        public async Task<Model> GetModelByIdAsync(int id)
        {
            var model = await _context.Models
                .Include(m => m.OrderModels)
                .ThenInclude(om => om.Order)
                .ThenInclude(o => o.Trader)
                .FirstOrDefaultAsync(m => m.Id == id);

            return model;
        }

        public async Task<IEnumerable<Model>> GetModelsByFilterAsync(string? modelName)
        {
            var query = _context.Models
                .Include(m => m.OrderModels)
                .ThenInclude(om => om.Order)
                .ThenInclude(o => o.Trader)
                .AsQueryable();

            if (!string.IsNullOrEmpty(modelName))
                query = query.Where(m => m.Model_Name.Contains(modelName));

            return await query.ToListAsync();
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
