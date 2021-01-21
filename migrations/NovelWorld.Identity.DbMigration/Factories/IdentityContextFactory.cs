using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using NovelWorld.Identity.Infrastructure.Contexts;

namespace NovelWorld.Identity.DbMigration.Factories
{
    public class IdentityContextFactory: IDesignTimeDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<IdentityDbContext>()
                .UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    x=>x.MigrationsAssembly("NovelWorld.Identity.DbMigration"));

            return new IdentityDbContext(builder.Options, null, null);
        }
        
        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}