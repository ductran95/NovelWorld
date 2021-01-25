using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NovelWorld.Infrastructure.EventSourcing.Abstractions;
using NovelWorld.Infrastructure.EventSourcing.Implements;
using NovelWorld.Infrastructure.Factories.Abstractions;
using NovelWorld.Infrastructure.Factories.Implements;

namespace NovelWorld.Infrastructure.Mappings
{
    public static class ServiceMapping
    {
        public static IServiceCollection RegisterDefaultDbConnectionFactory(this IServiceCollection services, string connectionString)
        {
            services.TryAddSingleton<IDbConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
            // ReSharper disable once PossibleNullReferenceException
            services.TryAddScoped(sp => sp.GetService<IDbConnectionFactory>().CreateConnection());
            return services;
        }
        
        public static IServiceCollection RegisterDefaultEventSourcing(this IServiceCollection services)
        {
            services.TryAddScoped<IDbEventSource, DbEventSource>();
            
            return services;
        }
    }
}
