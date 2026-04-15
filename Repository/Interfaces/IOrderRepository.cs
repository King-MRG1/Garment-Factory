using Database.Models;

namespace Repository.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        public Task<Order> GetOrderByIdAsync(int id);
        public Task<IEnumerable<Order>> GetOrdersByFilterAsync(string traderName, string modelName);
    }
}
