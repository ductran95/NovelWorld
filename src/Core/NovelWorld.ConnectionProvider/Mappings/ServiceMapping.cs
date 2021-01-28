using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NovelWorld.ConnectionProvider.Factories.Abstractions;

namespace NovelWorld.ConnectionProvider.Mappings
{
    public static class ServiceMapping
    {
        public static IServiceCollection RegisterDefaultDbConnection(this IServiceCollection services)
        {
            // ReSharper disable once PossibleNullReferenceException
            services.TryAddScoped(sp => sp.GetService<IDbConnectionFactory>().CreateConnection());
            return services;
        }
    }
}
