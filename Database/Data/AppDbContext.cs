using Database.Data.Configurations;
using Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Database.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppDbContext(DbContextOptions<AppDbContext> options,
                    IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private string? CurrentUserId =>
            _httpContextAccessor.HttpContext?.User
            .FindFirstValue(ClaimTypes.NameIdentifier);

        public DbSet<Model> Models { get; set; }
        public DbSet<Trader> Traders { get; set; }
        public DbSet<Worker> Worker { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderModel> OrderModels { get; set; }
        public DbSet<Fabric> Fabrics { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Revenue> Revenues { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<AdvanceAndDeduction> AdvanceAndDeductions { get; set; }
        public DbSet<RefreshTokenStore> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            builder.AddMultiTenantFilters(() => CurrentUserId);
        }
    }
}
