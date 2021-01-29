using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NovelWorld.ConnectionProvider.Factories.Abstractions;
using NovelWorld.ConnectionProvider.SqlServer.Factories.Implements;

namespace NovelWorld.ConnectionProvider.SqlServer.Mappings
{
    public static class ServiceMapping
    {
        public static IServiceCollection RegisterSqlServerDbConnectionFactory(this IServiceCollection services)
        {
            services.TryAddSingleton<IDbConnectionFactory, SqlConnectionFactory>();
            return services;
        }
    }
}
