using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseCreation.Models
{
    public class Revenue : IUserOwned
    {
        public int Id { get; set; }
        public string Revenue_Name { get; set; }
        public string Revenue_Description { get; set; }
        public decimal Amount { get; set; }
        public DateOnly Revenue_Date { get; set; }
        public DateOnly Last_Update { get; set; }
        public int? Trader_Id { get; set; }
        public Trader? Trader { get; set; }
        public string UserId { get; set; }
    }
}
