using Database.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Database.Data.Configurations
{
    public static class QueryFilterExtensions
    {
        public static void AddMultiTenantFilters(this ModelBuilder modelBuilder, 
            AppDbContext context)
        {
            var userOwnedType = typeof(IUserOwned);
            var entityTypes = modelBuilder.Model.GetEntityTypes()
                .Where(e => userOwnedType.IsAssignableFrom(e.ClrType))
                .ToList();

            foreach (var entityType in entityTypes)
            {
                // Use reflection to call generic method for this entity type
                var method = typeof(QueryFilterExtensions)
                    .GetMethod(nameof(ConfigureUserFilter),
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                    ?.MakeGenericMethod(entityType.ClrType)
                    ?? throw new InvalidOperationException(
                        $"Could not find ConfigureUserFilter method for {entityType.Name}");

                method.Invoke(null, new object?[] { modelBuilder, context });
            }
        }
        private static void ConfigureUserFilter<T>(ModelBuilder modelBuilder, 
            AppDbContext context) 
            where T : class, IUserOwned
        {
            modelBuilder.Entity<T>()
                .HasQueryFilter(e => e.UserId == context.CurrentUserId);
        }
    }
}