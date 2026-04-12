using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Database.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var basePath = FindApiProjectPath();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{GetEnvironment()}.json", optional: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    $"Connection string 'DefaultConnection' not found in appsettings.json at: {basePath}");
            }

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options, null);
        }
        private static string FindApiProjectPath()
        {
            var currentDir = Directory.GetCurrentDirectory();

            if (File.Exists(Path.Combine(currentDir, "appsettings.json")))
                return currentDir;

            var apiPath = Path.Combine(currentDir, "..", "API");
            if (File.Exists(Path.Combine(apiPath, "appsettings.json")))
                return Path.GetFullPath(apiPath);

            var apiSubPath = Path.Combine(currentDir, "API");
            if (File.Exists(Path.Combine(apiSubPath, "appsettings.json")))
                return apiSubPath;

            throw new InvalidOperationException(
                $"Could not find appsettings.json. Searched in: {currentDir}, {apiPath}, {apiSubPath}");
        }

        private static string GetEnvironment()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        }
    }
}
