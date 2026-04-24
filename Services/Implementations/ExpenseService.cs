using Database.Models;
using Microsoft.Extensions.Logging;
using Repository.Interfaces;
using Services.Interfaces;
using Shared.Dtos.ExpenseDtos;
using Shared.Dtos.QueryFilters;
using Shared.Helper;
using Shared.Interfaces;
using Shared.Mapping;

namespace Services.Implementations
{
    public class ExpenseService : IExpenseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<ExpenseService> _logger;

        public ExpenseService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, ILogger<ExpenseService> logger)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<ViewExpenseDto?> AddExpenseAsync(CreateExpenseDto createExpenseDto)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Creating new expense", userContext);

                var expense = createExpenseDto.ToExpense();

                if (expense == null)
                    throw new InvalidOperationException("Failed to map expense DTO to entity.");

                if (expense.Trader_Id.HasValue)
                    await EditAmountToTrader(expense.Trader_Id.Value, expense.Amount);

                var userId = _currentUserService.GetCurrentUserId();

                if (string.IsNullOrWhiteSpace(userId))
                    throw new InvalidOperationException("UserID not found in authentication context.");

                expense.UserId = userId;

                await _unitOfWork.Expenses.AddAsync(expense);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("{userContext} - Expense created successfully. Expense ID: {Id}", userContext, expense.Id);

                return await GetExpenseByIdAsync(expense.Id);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("{userContext} - Validation error: {Message}", userContext, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error creating expense: {Message}", userContext, ex.Message);
                throw;
            }
        }

        private async Task EditAmountToTrader(int? traderId, decimal amount)
        {
            var trader = await _unitOfWork.Traders.GetTraderByIdAsync(traderId);

            if (trader == null)
                return;

            if (trader.Trader_Type == TraderType.Customer)
                trader.Amount += amount;

            else if (trader.Trader_Type == TraderType.Supplier)
                trader.Amount -= amount;
        }

        public async Task<ViewExpenseDto?> DeleteExpenseAsync(int id)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Deleting expense {Id}", userContext, id);

                var expense = await _unitOfWork.Expenses.GetExpenseById(id);

                if (expense == null)
                {
                    _logger.LogWarning("{userContext} - Expense {Id} not found", userContext, id);
                    return null;
                }

                if (expense.Trader_Id.HasValue)
                    await EditAmountToTrader(expense.Trader_Id, -expense.Amount);

                _unitOfWork.Expenses.Delete(expense);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("{userContext} - Expense {Id} deleted successfully", userContext, id);

                return await GetExpenseByIdAsync(expense.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error deleting expense: {Message}", userContext, ex.Message);
                throw;
            }
        }

        public async Task<ViewExpenseDto?> GetExpenseByIdAsync(int id)
        {
            try
            {
                var expense = await _unitOfWork.Expenses.GetExpenseById(id);

                if (expense == null)
                    return null;

                return expense.ToExpenseDto();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving expense {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<ViewExpenseDto?> UpdateExpenseAsync(int id, UpdateExpenseDto updateExpenseDto)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Updating expense {Id}", userContext, id);

                var expense = await _unitOfWork.Expenses.GetExpenseById(id);

                if (expense == null)
                {
                    _logger.LogWarning("{userContext} - Expense {Id} not found", userContext, id);
                    return null;
                }

                if (updateExpenseDto.Trader_Id.HasValue && updateExpenseDto.Trader_Id != expense.Trader_Id)
                {
                    await EditAmountToTrader(expense.Trader_Id, -expense.Amount);

                    await EditAmountToTrader(updateExpenseDto.Trader_Id, updateExpenseDto.Amount);
                }
                else if (updateExpenseDto.Trader_Id.HasValue)
                {
                    await EditAmountToTrader(expense.Trader_Id, updateExpenseDto.Amount - expense.Amount);
                }

                expense.UpdateExpense(updateExpenseDto);

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("{userContext} - Expense {Id} updated successfully", userContext, id);

                return await GetExpenseByIdAsync(expense.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error updating expense: {Message}", userContext, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<ViewExpenseDto>> GetExpensesByFilterAsync(ExpenseFilter expenseFilter)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Retrieving expenses by filter", userContext);

                var expenses = await _unitOfWork.Expenses.GetExpensesByFilterAsync(traderName: expenseFilter.TraderName);

                _logger.LogInformation("{userContext} - Retrieved {Count} expenses", userContext, expenses.Count());

                return expenses.Select(e => e.ToExpenseDto());
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error retrieving expenses: {Message}", userContext, ex.Message);
                throw;
            }
        }
    }
}
