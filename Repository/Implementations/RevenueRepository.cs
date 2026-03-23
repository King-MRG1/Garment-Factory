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

        public async Task<IEnumerable<Revenue>> GetAllRevenueAsync()
        {
            var revenues = await _context.Revenues.Include(r => r.Trader).ToListAsync();

            return revenues;
        }

        public async Task<Revenue> GetRevenueByIdAsync(int id)
        {
            var revenue = await _context.Revenues.Include(r => r.Trader).FirstOrDefaultAsync(r => r.Id == id);

            return revenue;
        }

        public async Task<IEnumerable<Revenue>> GetRevenuesByTraderIdAsync(int traderId)
        {
            var revenues = await _context.Revenues.Include(r => r.Trader)
                .Where(r => r.Trader_Id == traderId).ToListAsync();

            return revenues;
        }
    }
}
