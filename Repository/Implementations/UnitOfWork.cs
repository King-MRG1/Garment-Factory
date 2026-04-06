using Database.Data;
using Repository.Interfaces;

namespace Repository.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        private IModelRepository? _models;
        private ITraderRepository? _traders;
        private IWorkerRepository? _workers;
        private IOrderRepository? _orders;
        private IFabricRepository? _fabrics;
        private IRevenueRepository? _revenues;
        private IExpenseRepository? _expenses;
        private IAdvanceAndDeductionRepository? _advanceAndDeductions;
        private IAuthRepository? _auth;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IModelRepository Models => _models ??= new ModelRepository(_context);
        public ITraderRepository Traders => _traders ??= new TraderRepository(_context);
        public IWorkerRepository Workers => _workers ??= new WorkerRepository(_context);
        public IOrderRepository Orders => _orders ??= new OrderRepository(_context);
        public IFabricRepository Fabrics => _fabrics ??= new FabricRepository(_context);
        public IRevenueRepository Revenues => _revenues ??= new RevenueRepository(_context);
        public IExpenseRepository Expenses => _expenses ??= new ExpenseRepository(_context);
        public IAdvanceAndDeductionRepository AdvanceAndDeductions => _advanceAndDeductions ??= new AdvanceAndDeductionRepository(_context);
        public IAuthRepository Auth => _auth ??= new AuthRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
