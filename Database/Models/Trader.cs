using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class Trader : IUserOwned
    {
        public int Id { get; set; }
        public string Trader_Name { get; set; }
        public string Address { get; set; }
        public TraderType Trader_Type { get; set; }
        public decimal Amount { get; set; }
        public DateOnly Register_date { get; set; }
        public DateOnly Last_Update { get; set; }
        public List<Phone> Phones { get; set; } = new List<Phone>();
        public List<Fabric> Fabrics { get; set; } = new List<Fabric>();
        public List<Revenue> Revenues { get; set; } = new List<Revenue>();
        public List<Expense> Expenses { get; set; } = new List<Expense>();
        public List<Order> Orders { get; set; } = new List<Order>();
        public string UserId { get; set; }
    }
   public enum TraderType
    {
        Supplier,
        Customer,
    }
}
