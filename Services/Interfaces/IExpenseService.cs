using Shared.Dtos.ExpenseDtos;
using Shared.Dtos.QueryFilters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IExpenseService
    {
        public Task<ViewExpenseDto?> GetExpenseByIdAsync(int id);
        public Task<IEnumerable<ViewExpenseDto>> GetAllExpenseAsync();
        public Task<IEnumerable<ViewExpenseDto>> GetExpensesByTraderIdAsync(int traderId);
        public Task<IEnumerable<ViewExpenseDto>> GetExpensesByFilterAsync(ExpenseFilter expenseFilter);
        public Task<ViewExpenseDto?> AddExpense(CreateExpenseDto createExpenseDto);
        public Task<ViewExpenseDto?> UpdateExpense(int id,UpdateExpenseDto updateExpenseDto);
        public Task<ViewExpenseDto?> DeleteExpense(int id);
    }
}
