using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseCreation.Models
{
    public class Expense : IUserOwned
    {
        public int Id { get; set; }
        public string Expense_Name { get; set; }
        public string Expense_Description { get; set; }
        public decimal Amount { get; set; }
        public DateOnly Expense_Date { get; set; }
        public DateOnly Last_Update { get; set; }
        public int? Trader_Id { get; set; }
        public Trader? Trader { get; set; }
        public string UserId { get; set; }
    }
}
