using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Shared.Dtos.ExpenseDtos
{
    public class ViewExpenseDto
    {
        public int Id { get; set; }
        public string Expense_Name { get; set; }
        public string Expense_Description { get; set; }
        public decimal Amount { get; set; }
        public DateOnly Expense_Date { get; set; }
        public DateOnly Last_Update { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Trader_Name { get; set; }
    }
}
