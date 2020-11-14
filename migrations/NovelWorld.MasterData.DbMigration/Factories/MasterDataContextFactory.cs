using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using NovelWorld.MasterData.Infrastructure.Contexts;

namespace NovelWorld.MasterData.DbMigration.Factories
{
    public class MasterDataContextFactory: IDesignTimeDbContextFactory<MasterDataContext>
    {
        public MasterDataContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<MasterDataContext>()
                .UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    x=>x.MigrationsAssembly("NovelWorld.MasterData.DbMigration"));

            return new MasterDataContext(builder.Options, null, null);
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