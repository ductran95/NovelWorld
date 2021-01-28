using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NovelWorld.Infrastructure.EventSourcing.Abstractions;
using NovelWorld.Infrastructure.EventSourcing.Implements;

namespace NovelWorld.Infrastructure.Mappings
{
    public static class ServiceMapping
    {
        public static IServiceCollection RegisterDefaultEventSourcing(this IServiceCollection services)
        {
            services.TryAddScoped<IDbEventSource, DbEventSource>();
            
            return services;
        }
    }
}
