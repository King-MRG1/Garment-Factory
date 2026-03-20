using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DatabaseCreation.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=MRG\\MSSQLSERVER01;Database=Garment_Factory;Trusted_Connection=True;TrustServerCertificate=True;");

            var httpContextAccessor = new HttpContextAccessor();

            return new AppDbContext(optionsBuilder.Options, httpContextAccessor);
        }
    }
}