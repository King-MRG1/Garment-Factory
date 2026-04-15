using Database.Models;

namespace Repository.Interfaces
{
    public interface IExpenseRepository : IGenericRepository<Expense>
    {
        public Task<Expense> GetExpenseById(int id);
        public Task<IEnumerable<Expense>> GetExpensesByFilterAsync(string? traderName);
    }
}
