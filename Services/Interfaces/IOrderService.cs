using Shared.Dtos.OrderDtos;
using Shared.Dtos.QueryFilters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IOrderService
    {
        public Task<ViewOrderDto?> GetOrderByIdAsync(int id);
        public Task<IEnumerable<ViewOrderDto>> GetOrdersByFilterAsync(OrderFilter orderFilter);
        public Task<ViewOrderDto?> CreateOrderAsync(CreateOrderDto createOrderDto);
        public Task<ViewOrderDto?> UpdateOrderAsync(int id,UpdateOrderDto updateOrderDto);
        public Task<ViewOrderDto?> DeleteOrderAsync(int id);

    }
}
