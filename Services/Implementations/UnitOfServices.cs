using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Implementations
{
    public class UnitOfServices : IUnitOfServices
    {
        public IWorkerService Workers { get;}
        public IFabricService Fabrics { get;}
        public IOrderService Orders { get;}
        public IExpenseService Expenses { get;}
        public ITraderService Traders { get;}   
        public IRevenueService Revenues { get;}
        public IModelService Models { get;}
        public IAuthService Auth { get; }
        public IAdvanceAndDeductionService AdvancesAndDeductions { get; }

        public UnitOfServices(IWorkerService workers,
            IFabricService fabrics,
            IOrderService orders, 
            IExpenseService expenses,
            ITraderService traders, 
            IRevenueService revenues,
            IModelService models,
            IAdvanceAndDeductionService advanceAndDeduction,
            IAuthService auth)

        {
            Workers = workers;
            Fabrics = fabrics;
            Orders = orders;
            Traders = traders;
            Expenses = expenses;
            Revenues = revenues;
            Models = models;
            Auth = auth;
            AdvancesAndDeductions = advanceAndDeduction;
        }
    }
}
