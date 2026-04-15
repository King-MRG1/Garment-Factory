using Database.Data;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;

namespace Repository.Implementations
{
    public class WorkerRepository : GenericRepository<Worker>, IWorkerRepository
    {
        public WorkerRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Worker> GetWorkerByIdAsync(int id)
        {
           var worker = await _context.Worker.Include(w => w.Phones)
                .Include(w => w.AdvanceAndDeductions)
                .FirstOrDefaultAsync(w => w.Id == id);
            return worker;
        }

        public async Task<IEnumerable<Worker>> GetWorkersByFilterAsync(string? name, int? type)
        {
            var query = _context.Worker.Include(w => w.Phones)
                .Include(w => w.AdvanceAndDeductions)
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(w => w.Worker_Name.Contains(name));

            if(type >= 0)
                query = query.Where(w => w.Worker_Type == (WorkerType)type);
            

            return await query.ToListAsync();
        }
    }
}
