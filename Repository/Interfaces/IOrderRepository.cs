using Database.Models;

namespace Repository.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        public Task<IEnumerable<Order>> GetOrdersAsync();
        public Task<Order> GetOrderByIdAsync(int id);
        public Task<IEnumerable<Order>> GetOrdersByTraderIdAsync(int traderId);
        public Task<IEnumerable<Order>> GetOrdersByModelIdAsync(int modelId);
    }
}
