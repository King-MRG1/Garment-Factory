using Database.Data;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;

namespace Repository.Implementations
{
    public class ExpenseRepository : GenericRepository<Expense>, IExpenseRepository
    {
        public ExpenseRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Expense>> GetAllExpensesAsync()
        {
            var expenses = await _context.Expenses.Include(e => e.Trader).ToListAsync();

            return expenses;
        }

        public async Task<Expense> GetExpenseById(int id)
        {
            var expense = await _context.Expenses.Include(e => e.Trader).FirstOrDefaultAsync(e => e.Id == id);

            return expense;
        }

        public async Task<IEnumerable<Expense>> GetExpensesByFilterAsync(string? traderName)
        {
            var query = _context.Expenses.Include(e => e.Trader).AsQueryable();

            if(!string.IsNullOrEmpty(traderName))
            {
                query = query.Where(e => e.Expense_Name.Contains(traderName));
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Expense>> GetExpensesByTraderId(int traderId)
        {
            var expenses = await _context.Expenses.Include(e => e.Trader).Where(e => e.Trader_Id == traderId).ToListAsync();

            return expenses;
        }
    }
}
