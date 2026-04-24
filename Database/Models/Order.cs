using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class Order : IUserOwned
    {
        public int Id { get; set; }
        public decimal Total_Cost { get; set; }
        public int Total_Quantity { get; set; }
        public DateOnly Order_Date { get; set; }
        public int Trader_Id { get; set; }
        public Trader Trader { get; set; }
        public List<OrderModel> OrderModels { get; set; } = new List<OrderModel>();
        public string UserId { get; set; }
    }
}
