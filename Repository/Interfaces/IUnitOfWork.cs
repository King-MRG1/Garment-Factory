namespace Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IModelRepository Models { get; }
        ITraderRepository Traders { get; }
        IWorkerRepository Workers { get; }
        IOrderRepository Orders { get; }
        IOrderModelRepository OrderModels { get; }
        IFabricRepository Fabrics { get; }
        IPhoneRepository Phones { get; }
        IRevenueRepository Revenues { get; }
        IExpenseRepository Expenses { get; }
        IAdvanceAndDeductionRepository AdvanceAndDeductions { get; }
        IAuthRepository Auth { get; }
        Task<int> SaveChangesAsync();
    }
}
