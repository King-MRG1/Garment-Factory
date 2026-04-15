using Database.Data;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;

namespace Repository.Implementations
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Trader)
                .Include(o => o.OrderModels)
                .ThenInclude(om => om.Model)
                .FirstOrDefaultAsync(o => o.Id == id);

            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersByFilterAsync(string traderName, string modelName)
        {
           var query = _context.Orders
                .Include(o => o.Trader)
                .Include(o => o.OrderModels)
                .ThenInclude(om => om.Model)
                .AsQueryable();

            if (!string.IsNullOrEmpty(traderName))
            {
                query = query.Where(o => o.Trader.Trader_Name.Contains(traderName));
            }

            if (!string.IsNullOrEmpty(modelName))
            {
                query = query.Where(o => o.OrderModels.Any(om => om.Model.Model_Name.Contains(modelName)));
            }
            return await query.ToListAsync();
        }
    }
}
