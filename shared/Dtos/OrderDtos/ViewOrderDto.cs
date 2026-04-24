using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Dtos.OrderDtos
{
    public class ViewOrderDto
    {
        public int Id { get; set; }
        public int Total_Quantity { get; set; }
        public decimal Total_Cost { get; set; }
        public DateOnly Order_Date { get; set; }
        public string Trader_Name { get; set; }
        public List<ViewOrderModelDto> OrderModels { get; set; } = new List<ViewOrderModelDto>();
    }
}
