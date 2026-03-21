using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class OrderModel : IUserOwned
    {
        public int Model_Id { get; set; }
        public int Order_Id { get; set; }
        public int Quantity { get; set; }
        public Model Model { get; set; }
        public Order Order { get; set; }
        public string UserId { get; set; }
    }
}
