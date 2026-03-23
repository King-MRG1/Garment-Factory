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
    }
}
