using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NovelWorld.EventBus.EventHandlers;

namespace NovelWorld.EventBus.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterIntegrationEventHandler(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            if (!assemblies.Any())
            {
                throw new ArgumentException("No assemblies found to scan. Supply at least one assembly to scan for handlers.");
            }

            GetIntegrationEventHandler(typeof(IntegrationEventHandler<>), services, assemblies);
            GetIntegrationEventHandler(typeof(DynamicIntegrationEventHandler), services, assemblies);

            return services;
        }

        public static IServiceCollection RegisterIntegrationEventHandler(this IServiceCollection services, IEnumerable<Type> handlerAssemblyMarkerTypes)
            => services.RegisterIntegrationEventHandler(handlerAssemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly));

        private static void GetIntegrationEventHandler(Type openRequestInterface,
            IServiceCollection services,
            IEnumerable<Assembly> assembliesToScan)
        {
            var types = assembliesToScan.SelectMany(x => x.DefinedTypes);

            foreach(var type in types)
            {
                var interfaces = type.GetInterfaces();

                if (openRequestInterface.IsGenericType)
                {
                    var genericInterfaces = interfaces.Where(x => x.IsGenericType);
                    if (genericInterfaces.Any(x => x.GetGenericTypeDefinition() == openRequestInterface))
                    {
                        services.TryAddTransient(type);
                    }
                }
                else
                {
                    if(interfaces.Any(x => x == openRequestInterface))
                    {
                        services.TryAddTransient(type);
                    }
                }
            }
        }

    }
}
