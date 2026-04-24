using Database.Data;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;

namespace Repository.Implementations
{
    public class TraderRepository : GenericRepository<Trader>, ITraderRepository
    {
        public TraderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Trader> GetTraderByIdAsync(int? id)
        {
            var trader = await _context.Traders
                .Include(Trader => Trader.Phones)
                .Include(Trader => Trader.Fabrics)
                .Include(Trader => Trader.Revenues)
                .Include(Trader => Trader.Expenses)
                .Include(Trader => Trader.Orders)
                .ThenInclude(o => o.OrderModels)
                .ThenInclude(om => om.Model)
                .FirstOrDefaultAsync(t => t.Id == id);

            return trader;
        }

        public async Task<IEnumerable<Trader>> GetTradersByFilterAsync(string? name, int? type)
        {
            var query = _context.Traders
                .Include(Trader => Trader.Phones)
                .Include(Trader => Trader.Fabrics)
                .Include(Trader => Trader.Revenues)
                .Include(Trader => Trader.Expenses)
                .Include(Trader => Trader.Orders)
                .ThenInclude(o => o.OrderModels)
                .ThenInclude(om => om.Model)
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(t => t.Trader_Name.Contains(name));
            }

            if(type >= 0)
            {
                query = query.Where(t => t.Trader_Type == (TraderType)type);
            }

            return await query.ToListAsync();
        }
    }
}
