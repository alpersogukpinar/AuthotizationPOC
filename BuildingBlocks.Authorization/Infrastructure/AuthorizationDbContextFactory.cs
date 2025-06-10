using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BuildingBlocks.Authorization.Infrastructure
{
    public class AuthorizationDbContextFactory : IDesignTimeDbContextFactory<AuthorizationDbContext>
    {
        public AuthorizationDbContext CreateDbContext(string[] args)
        {
            // appsettings.json'u bulmak için yol ayarlanıyor
            var basePath = Directory.GetCurrentDirectory();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = configuration.GetConnectionString("AuthorizationDb");

            var optionsBuilder = new DbContextOptionsBuilder<AuthorizationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new AuthorizationDbContext(optionsBuilder.Options);
        }
    }
}