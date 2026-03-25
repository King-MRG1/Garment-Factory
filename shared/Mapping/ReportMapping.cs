using Shared.Dtos.ExpenseDtos;
using Shared.Dtos.ReportsDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Mapping
{
    public static class ReportMapping
    {
        public static CreateExpenseDto ToCreateExpenseDto(this BaseWeeklyPaymentDto payment)
        {
            return new CreateExpenseDto
            {
                Expense_Name = $"Weekly Payment - {payment.Worker_Name}",
                Expense_Description = $"Worker Type: {payment.Worker_Type} | " +
                                      $"Earned: {payment.TotalEarned} | " +
                                      $"Advances: {payment.TotalAdvances} | " +
                                      $"Deductions: {payment.TotalDeductions}",
                Amount = payment.NetPayment,
                Trader_Id = null
            };
        }
    }
}
