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

        public async Task<IEnumerable<Worker>> GetWorkersAsync()
        {
            var workers = await _context.Worker.Include(w => w.Phones)
                .Include(w => w.AdvanceAndDeductions)
                .ToListAsync();

            return workers;
        }

        public async Task<IEnumerable<Worker>> GetWorkersByNameAsync(string name)
        {
            var workers = await _context.Worker.Include(w => w.Phones)
                .Include(w => w.AdvanceAndDeductions)
                .Where(w => w.Worker_Name.Contains(name))
                .ToListAsync();

            return workers;
        }

        public async Task<IEnumerable<Worker>> GetWorkersByTypeAsync(WorkerType type)
        {
            var workers = await _context.Worker.Include(w => w.Phones)
                .Include(w => w.AdvanceAndDeductions)
                .Where(w => w.Worker_Type == type)
                .ToListAsync();
              
            return workers;
        }
    }
}
