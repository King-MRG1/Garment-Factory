using Database.Data;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;

namespace Repository.Implementations
{
    public class AdvanceAndDeductionRepository : GenericRepository<AdvanceAndDeduction>, IAdvanceAndDeductionRepository
    {
        public AdvanceAndDeductionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<AdvanceAndDeduction> GetAdvanceAndDeductionsByIdAsync(int id)
        {
            var AdvanceOrDeduction = await _context.AdvanceAndDeductions
                .Include(a => a.Worker)
                .FirstOrDefaultAsync(a => a.Id == id);

            return AdvanceOrDeduction;
        }

        public async Task<IEnumerable<AdvanceAndDeduction>> GetAdvancesAndDeductionsByFilterAsync(int type, DateOnly startDate, DateOnly endDate, string workerName)
        {
            var query = _context.AdvanceAndDeductions
                .Include(a => a.Worker)
                .AsQueryable();

            if (type != 0)
                query = query.Where(a => a.Type == (AdvanceOrDeduction)type);

            if (startDate != default)
                query = query.Where(a => a.Date >= startDate);

            if (endDate != default)
                query = query.Where(a => a.Date <= endDate);

            if (string.IsNullOrEmpty(workerName) == false)
                query = query.Where(a => a.Worker.Worker_Name.Contains(workerName));

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<AdvanceAndDeduction>> GetAllAdvanceAndDeductionsAsync()
        {
            var AdvanceOrDeductions = await _context.AdvanceAndDeductions
                .Include(a => a.Worker)
                .ToListAsync();

            return AdvanceOrDeductions;
        }

        public async Task<IEnumerable<AdvanceAndDeduction>> GetAllAdvanceAndDeductionsByTypeAsync(int type)
        {
            var AdvancesOrDeductions = await _context.AdvanceAndDeductions
                .Where(a => a.Type == (AdvanceOrDeduction)type)
                .ToListAsync();

            return AdvancesOrDeductions;
        }

        public async Task MakeAdvanceAndDeductionUsed(List<int> advanceAndDeductionIds)
        {
            var advancesAndDeductions = await _context.AdvanceAndDeductions
                .Where(a => advanceAndDeductionIds.Contains(a.Id))
                .ToListAsync();

            foreach (var item in advancesAndDeductions)
            {
                item.IsUsed = true;
            }

            await _context.SaveChangesAsync();
        }
    }
}
