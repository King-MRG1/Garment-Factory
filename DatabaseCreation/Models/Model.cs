using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseCreation.Models
{
    public class Model : IUserOwned
    {
        public int Id { get; set; }
        public string Model_Name { get; set; }
        public decimal Price_Trader { get; set; }
        public decimal Price_Stitcher { get; set; }
        public decimal Price_Ironer { get; set; }
        public decimal Price_Cutter { get; set; }
        public int Total_Units { get; set; } = 0;
        public DateOnly Last_Update { get; set; }
        public List<OrderModel> OrderModels { get; set; } = new List<OrderModel>();
        public string UserId { get; set; }
    }
}
