using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.Dtos.ExpenseDtos
{
    public class UpdateExpenseDto
    {
        [Required(ErrorMessage = "Expense name is required")]
        [MaxLength(100, ErrorMessage = "Expense name cannot exceed 100 characters")]
        public string Expense_Name { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Expense_Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount { get; set; }

        [Required]
        public int? Trader_Id { get; set; }
    }
}
