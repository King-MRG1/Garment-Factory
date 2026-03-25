using Database.Models;
using Shared.Dtos.ExpenseDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Mapping
{
    public static class ExpenseMapping
    {
        public static ViewExpenseDto ToExpenseDto(this Expense expense)
        {
            return new ViewExpenseDto
            {
                Id = expense.Id,
                Expense_Name = expense.Expense_Name,
                Expense_Description = expense.Expense_Description,
                Amount = expense.Amount,
                Expense_Date = expense.Expense_Date,
                Last_Update = expense.Last_Update,
                Trader_Name = expense.Trader != null ? expense.Trader.Trader_Name : null
            };
        }
        public static Expense ToExpense(this CreateExpenseDto createExpenseDto)
        {
            return new Expense
            {
                Expense_Name = createExpenseDto.Expense_Name,
                Expense_Description = createExpenseDto.Expense_Description,
                Amount = createExpenseDto.Amount,
                Expense_Date = DateOnly.FromDateTime(DateTime.Now),
                Last_Update = DateOnly.FromDateTime(DateTime.Now),
                Trader_Id = createExpenseDto.Trader_Id != null ? createExpenseDto.Trader_Id : null
            };
        }
        public static void UpdateExpense(this Expense expense, UpdateExpenseDto updateExpenseDto)
        {
            expense.Expense_Name = updateExpenseDto.Expense_Name;
            expense.Expense_Description = updateExpenseDto.Expense_Description;
            expense.Amount = updateExpenseDto.Amount;
            expense.Trader_Id = updateExpenseDto.Trader_Id;
            expense.Last_Update = DateOnly.FromDateTime(DateTime.Now);
        }
    }
}
