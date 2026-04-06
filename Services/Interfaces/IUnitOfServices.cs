using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IUnitOfServices
    {
        IWorkerService Workers { get; }
        IFabricService Fabrics { get; }
        IOrderService Orders { get; }
        IExpenseService Expenses { get; }
        ITraderService Traders { get; }
        IRevenueService Revenues { get; }
        IModelService Models { get; }
        IAuthService Auth { get; }
        IAdvanceAndDeductionService AdvancesAndDeductions { get; }

    }
}
