using System.Data.Common;
using Microsoft.Extensions.Options;
using NovelWorld.ConnectionProvider.Configurations;
using NovelWorld.ConnectionProvider.Factories.Abstractions;
using Npgsql;

namespace NovelWorld.ConnectionProvider.PostgreSql.Factories.Implements
{
    public class NpgsqlConnectionFactory: IDbConnectionFactory
    {
        protected readonly DbConfiguration _configuration;

        public DbConfiguration Configuration => _configuration;
        
        public NpgsqlConnectionFactory(IOptions<DbConfiguration> configuration)
        {
            _configuration = configuration.Value;
        }

        public DbConnection CreateConnection()
        {
            return new NpgsqlConnection(_configuration.ConnectionString);
        }
    }
}