using System.Data.Common;

namespace NovelWorld.ConnectionProvider.Factories.Abstractions
{
    public interface IDbConnectionFactory
    {
        string ConnectionString { get; }
        DbConnection CreateConnection();
    }
}