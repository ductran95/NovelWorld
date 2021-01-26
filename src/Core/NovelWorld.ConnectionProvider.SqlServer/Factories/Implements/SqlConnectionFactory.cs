using System.Data.Common;
using System.Data.SqlClient;
using NovelWorld.ConnectionProvider.Factories.Abstractions;

namespace NovelWorld.ConnectionProvider.SqlServer.Factories.Implements
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