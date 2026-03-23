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

        public async Task<Trader> GetTraderByIdAsync(int id)
        {
            var trader = await _context.Traders
                .Include(Trader => Trader.Phones)
                .Include(Trader => Trader.Fabrics)
                .Include(Trader => Trader.Revenues)
                .Include(Trader => Trader.Expenses)
                .Include(Trader => Trader.Orders)
                .FirstOrDefaultAsync(t => t.Id == id);

            return trader;
        }

        public async Task<IEnumerable<Trader>> GetTraderByNameAsync(string name)
        {
            var trader = await _context.Traders
                .Include(Trader => Trader.Phones)
                .Include(Trader => Trader.Fabrics)
                .Include(Trader => Trader.Revenues)
                .Include(Trader => Trader.Expenses)
                .Include(Trader => Trader.Orders)
                .Where(t => t.Trader_Name.Contains(name)).ToListAsync();

            return trader;
        }

        public async Task<IEnumerable<Trader>> GetTraderByTypeAsync(TraderType type)
        {
            var trader = await _context.Traders
                .Include(Trader => Trader.Phones)
                .Include(Trader => Trader.Fabrics)
                .Include(Trader => Trader.Revenues)
                .Include(Trader => Trader.Expenses)
                .Include(Trader => Trader.Orders)
                .Where(t => t.Trader_Type == type).ToListAsync();

            return trader;
        }

        public async Task<IEnumerable<Trader>> GetTradersAsync()
        {
            var traders = await _context.Traders
                .Include(Trader => Trader.Phones)
                .Include(Trader => Trader.Fabrics)
                .Include(Trader => Trader.Revenues)
                .Include(Trader => Trader.Expenses)
                .Include(Trader => Trader.Orders)
                .ToListAsync();

            return traders;
        }
    }
}
