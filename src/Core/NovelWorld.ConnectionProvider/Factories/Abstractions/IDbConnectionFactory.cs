using System.Data.Common;
using NovelWorld.ConnectionProvider.Configurations;

namespace NovelWorld.ConnectionProvider.Factories.Abstractions
{
    public interface IDbConnectionFactory
    {
        DbConfiguration Configuration { get; }
        DbConnection CreateConnection();
    }
}