using Database.Models;
using Shared.Dtos.OrderDtos;

public static class OrderMapping
{
    public static ViewOrderDto ToOrderDto(this Order order)
    {
        return new ViewOrderDto
        {
            Id = order.Id,
            Total_Cost = order.Total_Cost,
            Order_Date = order.Order_Date,
            Trader_Name = order.Trader != null ? order.Trader.Trader_Name : string.Empty,
        };
    }
    public static Order ToOrder(this CreateOrderDto createOrderDto)
    {
        return new Order
        {
            Order_Date = DateOnly.FromDateTime(DateTime.Now),
            Trader_Id = createOrderDto.Trader_Id,
            OrderModels = createOrderDto.OrderModels.ConvertAll(om => new OrderModel
            {
                Model_Id = om.Model_Id,
                Quantity = om.Quantity
            })
        };
    }
    public static void UpdateOrder(this Order order, UpdateOrderDto updateOrderDto)
    {
        order.Total_Cost = updateOrderDto.Total_Cost;
        order.Trader_Id = updateOrderDto.Trader_Id;
        order.OrderModels.Clear();
        order.OrderModels.AddRange(updateOrderDto.OrderModels.ConvertAll(om => new OrderModel
        {
            Order_Id = order.Id,
            Model_Id = om.Model_Id,
            Quantity = om.Quantity
        }));
    }
}
