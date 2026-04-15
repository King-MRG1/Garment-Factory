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

    }
}
