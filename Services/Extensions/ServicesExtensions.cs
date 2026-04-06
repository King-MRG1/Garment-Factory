using Microsoft.Extensions.DependencyInjection;
using Services.Implementations;
using Services.Interfaces;

namespace Services.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAdvanceAndDeductionService, AdvanceAndDeductionService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<IFabricService, FabricService>();
            services.AddScoped<IModelService, ModelService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IRevenueService, RevenueService>();
            services.AddScoped<ITraderService, TraderService>();
            services.AddScoped<IWorkerService, WorkerService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddScoped<IUnitOfServices, UnitOfServices>();

            return services;
        }
    }
}
