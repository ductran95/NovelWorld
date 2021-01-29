using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Extensions.Options;
using NovelWorld.ConnectionProvider.Configurations;
using NovelWorld.ConnectionProvider.Factories.Abstractions;

namespace NovelWorld.ConnectionProvider.SqlServer.Factories.Implements
{
    public class SqlConnectionFactory: IDbConnectionFactory
    {
        protected readonly DbConfiguration _configuration;

        public DbConfiguration Configuration => _configuration;
        
        public SqlConnectionFactory(IOptions<DbConfiguration> configuration)
        {
            _configuration = configuration.Value;
        }

        public DbConnection CreateConnection()
        {
            return new SqlConnection(_configuration.ConnectionString);
        }
    }
}