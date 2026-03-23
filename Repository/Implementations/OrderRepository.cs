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

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            var orders = await _context.Orders
            .Include(o => o.Trader)
            .Include(o => o.OrderModels)
            .ThenInclude(om => om.Model)
            .ToListAsync();

            return orders;
        }

        public async Task<IEnumerable<Order>> GetOrdersByTraderIdAsync(int traderId)
        {
            var orders = await _context.Orders
                .Where(o => o.Trader_Id == traderId)
                .Include(o => o.Trader)
                .Include(o => o.OrderModels)
                .ThenInclude(om => om.Model)
                .ToListAsync();

            return orders;
        }
        public async Task<IEnumerable<Order>> GetOrdersByModelIdAsync(int modelId)
        {
            var orders = await _context.Orders
                .Where(o => o.OrderModels.Any(om => om.Model_Id == modelId))
                .Include(o => o.Trader)
                .Include(o => o.OrderModels)
                .ThenInclude(om => om.Model)
                .ToListAsync();

            return orders;
        }
    }
}
