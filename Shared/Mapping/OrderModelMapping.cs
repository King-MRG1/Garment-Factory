using Database.Models;
using Shared.Dtos.OrderDtos;

public static class OrderModelMapping
{
    public static ViewOrderModelDto ToOrderModelDto(this OrderModel orderModel)
    {
        return new ViewOrderModelDto
        {
            Model_Id = orderModel.Model_Id,
            Order_Id = orderModel.Order_Id,
            Model_Name = orderModel.Model != null ? orderModel.Model.Model_Name : string.Empty,
            Quantity = orderModel.Quantity,
            Price  = orderModel.Price,
            
        };
    }
    public static ViewOrderModelDto ToOrderModelDto(this OrderModel orderModel , Model model)
    {
        return new ViewOrderModelDto
        {
            Model_Id = orderModel.Model_Id,
            Order_Id = orderModel.Order_Id,
            Model_Name = model != null ? model.Model_Name : string.Empty,
            Quantity = orderModel.Quantity,
            Price = orderModel.Price,

        };
    }
}
