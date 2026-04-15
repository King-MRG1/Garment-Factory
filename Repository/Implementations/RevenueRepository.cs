using Database.Data;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;

namespace Repository.Implementations
{
    public class RevenueRepository : GenericRepository<Revenue>, IRevenueRepository
    {
        public RevenueRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Revenue> GetRevenueByIdAsync(int id)
        {
            var revenue = await _context.Revenues.Include(r => r.Trader).FirstOrDefaultAsync(r => r.Id == id);

            return revenue;
        }

        public async Task<IEnumerable<Revenue>> GetRevenuesByFilterAsync(string? traderName)
        {
            var query = _context.Revenues.Include(r => r.Trader).AsQueryable();

            if (!string.IsNullOrEmpty(traderName))
            {
                query = query.Where(r => r.Trader != null && r.Trader.Trader_Name.Contains(traderName));
            }

            return await query.ToListAsync();
        }

    }
}
