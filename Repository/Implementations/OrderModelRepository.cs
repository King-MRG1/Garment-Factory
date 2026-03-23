using Database.Data;
using Database.Models;
using Repository.Interfaces;

namespace Repository.Implementations
{
    public class OrderModelRepository : GenericRepository<OrderModel>, IOrderModelRepository
    {
        public OrderModelRepository(AppDbContext context) : base(context)
        {
        }
    }
}
