using Database.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Repository.Interfaces;
using Services.Interfaces;
using Shared.Dtos.ExpenseDtos;
using Shared.Dtos.QueryFilters;
using Shared.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Implementations
{
    public class ExpenseService : IExpenseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        public ExpenseService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ViewExpenseDto?> AddExpenseAsync(CreateExpenseDto createExpenseDto)
        {
            var expense = createExpenseDto.ToExpense();

            if(expense.Trader_Id.HasValue)
                await EditAmountToTrader(expense.Trader_Id.Value, expense.Amount);
            
            var userId = _currentUserService.GetCurrentUserId();

            if(string.IsNullOrWhiteSpace(userId))
                throw new InvalidOperationException("UserID not found in authentication context.");

            expense.UserId = userId;

            await _unitOfWork.Expenses.AddAsync(expense);
            await _unitOfWork.SaveChangesAsync();

            return expense.ToExpenseDto();
        }

        private async Task EditAmountToTrader(int traderId, decimal amount)
        {
            var trader = await _unitOfWork.Traders.GetTraderByIdAsync(traderId);

            if(trader == null)
                return;

            if (trader.Trader_Type == TraderType.Customer)
                trader.Amount += amount;

            else if (trader.Trader_Type == TraderType.Supplier)
                trader.Amount -= amount;
        }

        public async Task<ViewExpenseDto?> DeleteExpenseAsync(int id)
        {
            var expense = await _unitOfWork.Expenses.GetExpenseById(id);

            if (expense == null)
                return null;

            _unitOfWork.Expenses.Delete(expense);
            await _unitOfWork.SaveChangesAsync();

            return expense.ToExpenseDto();
        }

        public async Task<IEnumerable<ViewExpenseDto>> GetAllExpenseAsync()
        {
            var expense = await _unitOfWork.Expenses.GetAllExpensesAsync();

            return expense.Select(e => e.ToExpenseDto());
        }

        public async Task<ViewExpenseDto?> GetExpenseByIdAsync(int id)
        {
            var expense = await _unitOfWork.Expenses.GetExpenseById(id);

            if (expense == null)
                return null;

            return expense.ToExpenseDto();
        }

        public async Task<IEnumerable<ViewExpenseDto>> GetExpensesByTraderIdAsync(int traderId)
        {
            var expenses = await _unitOfWork.Expenses.GetExpensesByTraderId(traderId);

            return expenses.Select(e => e.ToExpenseDto());
        }

        public async Task<ViewExpenseDto?> UpdateExpenseAsync(int id,UpdateExpenseDto updateExpenseDto)
        {
            var expense = await _unitOfWork.Expenses.GetExpenseById(id);

            if (expense == null)
                return null;

            expense.UpdateExpense(updateExpenseDto);

            await _unitOfWork.SaveChangesAsync();

            return expense.ToExpenseDto();
        }

        public async Task<IEnumerable<ViewExpenseDto>> GetExpensesByFilterAsync(ExpenseFilter expenseFilter)
        {
            var expenses = await _unitOfWork.Expenses.GetExpensesByFilterAsync(traderName: expenseFilter.TraderName);

            return expenses.Select(e => e.ToExpenseDto());
        }
    }
}
