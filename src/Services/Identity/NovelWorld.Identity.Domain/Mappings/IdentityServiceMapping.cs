using System;
using Microsoft.Extensions.DependencyInjection;
using NovelWorld.EventBus;

namespace NovelWorld.Identity.Domain.Mappings
{
    public static class IdentityServiceMapping
    {
        public static IServiceCollection RegisterServices(
            this IServiceCollection services)
        {

            return services;
        }
        
        public static IServiceProvider SubscribeIntegrationEvents(this IServiceProvider serviceProvider)
        {
            var eventBus = serviceProvider.GetRequiredService<IEventBus>();


            return serviceProvider;
        }
    }
}
