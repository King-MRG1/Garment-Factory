using Database.Models;

namespace Repository.Interfaces
{
    public interface IExpenseRepository : IGenericRepository<Expense>
    {
        public Task<IEnumerable<Expense>> GetAllExpensesAsync();
        public Task<Expense> GetExpenseById(int id);
        public Task<IEnumerable<Expense>> GetExpensesByTraderId(int traderId);
        public Task<IEnumerable<Expense>> GetExpensesByFilterAsync(string? traderName);
    }
}
