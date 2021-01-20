using System.Data.Common;

namespace NovelWorld.Infrastructure.Factories.Abstractions
{
    public interface IDbConnectionFactory
    {
        string ConnectionString { get; }
        DbConnection CreateConnection();
    }
}