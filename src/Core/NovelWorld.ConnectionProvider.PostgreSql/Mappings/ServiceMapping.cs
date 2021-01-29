using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NovelWorld.ConnectionProvider.Factories.Abstractions;
using NovelWorld.ConnectionProvider.PostgreSql.Factories.Implements;

namespace NovelWorld.ConnectionProvider.PostgreSql.Mappings
{
    public static class ServiceMapping
    {
        public static IServiceCollection RegisterPostgreSqlDbConnectionFactory(this IServiceCollection services)
        {
            services.TryAddSingleton<IDbConnectionFactory, NpgsqlConnectionFactory>();
            return services;
        }
    }
}
