using System.Data.Common;
using NovelWorld.ConnectionProvider.Factories.Abstractions;
using Npgsql;

namespace NovelWorld.ConnectionProvider.PostgreSql.Factories.Implements
{
    public class NpgsqlConnectionFactory: IDbConnectionFactory
    {
        protected readonly string _connectionString;

        public string ConnectionString => _connectionString;
        
        public NpgsqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbConnection CreateConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }
    }
}