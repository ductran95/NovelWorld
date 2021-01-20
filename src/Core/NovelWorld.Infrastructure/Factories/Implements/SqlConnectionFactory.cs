using System.Data.Common;
using System.Data.SqlClient;
using NovelWorld.Infrastructure.Factories.Abstractions;

namespace NovelWorld.Infrastructure.Factories.Implements
{
    public class SqlConnectionFactory: IDbConnectionFactory
    {
        protected readonly string _connectionString;

        public string ConnectionString => _connectionString;
        
        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}