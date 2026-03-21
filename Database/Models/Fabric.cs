using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class Fabric : IUserOwned
    {
        public int Id { get; set; }
        public string Fabric_Name { get; set; }
        public decimal Metres { get; set; }
        public decimal Price { get; set; }
        public DateOnly DateAdded { get; set; }
        public DateOnly Last_Update { get; set; }
        public int Trader_Id { get; set; }
        public Trader Trader { get; set; }
        public string UserId { get; set; }
    }
}
