using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NovelWorld.Utility.Attributes;

namespace NovelWorld.Mediator.DependencyInjection.Registration
{
    public static class ServiceRegistrar
    {
        public static void AddMediatRClasses(IServiceCollection services, IEnumerable<Assembly> assembliesToScan, MediatorServiceConfiguration serviceConfiguration)
        {
            assembliesToScan = (assembliesToScan as Assembly[] ?? assembliesToScan).Distinct().ToArray();

            ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>), services, assembliesToScan, false, serviceConfiguration);
            ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>), services, assembliesToScan, true, serviceConfiguration);
            ConnectImplementationsToTypesClosing(typeof(IRequestPreProcessor<>), services, assembliesToScan, true, serviceConfiguration);
            ConnectImplementationsToTypesClosing(typeof(IRequestPostProcessor<,>), services, assembliesToScan, true, serviceConfiguration);
            ConnectImplementationsToTypesClosing(typeof(IRequestExceptionHandler<,,>), services, assembliesToScan, true, serviceConfiguration);
            ConnectImplementationsToTypesClosing(typeof(IRequestExceptionAction<,>), services, assembliesToScan, true, serviceConfiguration);

            var multiOpenInterfaces = new[]
            {
                typeof(INotificationHandler<>),
                typeof(IRequestPreProcessor<>),
                typeof(IRequestPostProcessor<,>),
                typeof(IRequestExceptionHandler<,,>),
                typeof(IRequestExceptionAction<,>)
            };

            foreach (var multiOpenInterface in multiOpenInterfaces)
            {
                var concretions = assembliesToScan
                    .SelectMany(a => a.DefinedTypes)
                    .Where(type => type.FindInterfacesThatClose(multiOpenInterface).Any())
                    .Where(type => type.IsConcrete() && type.IsOpenGeneric() && type.CanAutoRegister())
                    .ToList();

                foreach (var type in concretions)
                {
                    services.Add(new ServiceDescriptor(multiOpenInterface, type, serviceConfiguration.HandlerLifetime));
                }
            }
        }

        /// <summary>
        /// Helper method use to differentiate behavior between request handlers and notification handlers.
        /// Request handlers should only be added once (so set addIfAlreadyExists to false)
        /// Notification handlers should all be added (set addIfAlreadyExists to true)
        /// </summary>
        /// <param name="openRequestInterface"></param>
        /// <param name="services"></param>
        /// <param name="assembliesToScan"></param>
        /// <param name="addIfAlreadyExists"></param>
        /// <param name="serviceConfiguration"></param>
        private static void ConnectImplementationsToTypesClosing(Type openRequestInterface,
            IServiceCollection services,
            IEnumerable<Assembly> assembliesToScan,
            bool addIfAlreadyExists, MediatorServiceConfiguration serviceConfiguration)
        {
            var concretions = new List<Type>();
            var interfaces = new List<Type>();
            foreach (var type in assembliesToScan.SelectMany(a => a.DefinedTypes).Where(t => !t.IsOpenGeneric()))
            {
                var interfaceTypes = type.FindInterfacesThatClose(openRequestInterface).ToArray();
                if (!interfaceTypes.Any()) continue;

                if (type.IsConcrete() && type.CanAutoRegister())
                {
                    concretions.Add(type);
                }

                foreach (var interfaceType in interfaceTypes)
                {
                    interfaces.Fill(interfaceType);
                }
            }

            foreach (var @interface in interfaces)
            {
                var exactMatches = concretions.Where(x => x.CanBeCastTo(@interface)).ToList();
                if (addIfAlreadyExists)
                {
                    foreach (var type in exactMatches)
                    {
                        services.Add(new ServiceDescriptor(@interface, type, serviceConfiguration.HandlerLifetime));
                    }
                }
                else
                {
                    if (exactMatches.Count > 1)
                    {
                        exactMatches.RemoveAll(m => !IsMatchingWithInterface(m, @interface));
                    }

                    foreach (var type in exactMatches)
                    {
                        services.TryAdd(new ServiceDescriptor(@interface, type, serviceConfiguration.HandlerLifetime));
                    }
                }

                if (!@interface.IsOpenGeneric())
                {
                    AddConcretionsThatCouldBeClosed(@interface, concretions, services, serviceConfiguration);
                }
            }
        }

        private static bool IsMatchingWithInterface(Type handlerType, Type handlerInterface)
        {
            if (handlerType == null || handlerInterface == null)
            {
                return false;
            }

            if (handlerType.IsInterface)
            {
                if (handlerType.GenericTypeArguments.SequenceEqual(handlerInterface.GenericTypeArguments))
                {
                    return true;
                }
            }
            else
            {
                return IsMatchingWithInterface(handlerType.GetInterface(handlerInterface.Name), handlerInterface);
            }

            return false;
        }

        private static void AddConcretionsThatCouldBeClosed(Type @interface, List<Type> concretions, IServiceCollection services, MediatorServiceConfiguration serviceConfiguration)
        {
            foreach (var type in concretions
                .Where(x => x.IsOpenGeneric() && x.CouldCloseTo(@interface)))
            {
                try
                {
                    services.TryAdd(new ServiceDescriptor(@interface, type.MakeGenericType(@interface.GenericTypeArguments), serviceConfiguration.HandlerLifetime));
                }
                catch (Exception)
                {
                }
            }
        }

        private static bool CouldCloseTo(this Type openConcretion, Type closedInterface)
        {
            var openInterface = closedInterface.GetGenericTypeDefinition();
            var arguments = closedInterface.GenericTypeArguments;

            var concreteArguments = openConcretion.GenericTypeArguments;
            return arguments.Length == concreteArguments.Length && openConcretion.CanBeCastTo(openInterface);
        }

        private static bool CanBeCastTo(this Type pluggedType, Type pluginType)
        {
            if (pluggedType == null) return false;

            if (pluggedType == pluginType) return true;

            return pluginType.GetTypeInfo().IsAssignableFrom(pluggedType.GetTypeInfo());
        }

        public static bool IsOpenGeneric(this Type type)
        {
            return type.GetTypeInfo().IsGenericTypeDefinition || type.GetTypeInfo().ContainsGenericParameters;
        }

        public static bool CanAutoRegister(this Type type)
        {
            var notAutoRegisterAttribute = type.GetCustomAttribute<NotAutoRegisterAttribute>();
            if (notAutoRegisterAttribute != null)
            {
                return false;
            }

            return true;
        }

        public static IEnumerable<Type> FindInterfacesThatClose(this Type pluggedType, Type templateType)
        {
            return FindInterfacesThatClosesCore(pluggedType, templateType).Distinct();
        }

        private static IEnumerable<Type> FindInterfacesThatClosesCore(Type pluggedType, Type templateType)
        {
            if (pluggedType == null) yield break;

            if (!pluggedType.IsConcrete()) yield break;

            if (templateType.GetTypeInfo().IsInterface)
            {
                foreach (
                    var interfaceType in
                    pluggedType.GetInterfaces()
                        .Where(type => type.GetTypeInfo().IsGenericType && (type.GetGenericTypeDefinition() == templateType)))
                {
                    yield return interfaceType;
                }
            }
            else if (pluggedType.GetTypeInfo().BaseType.GetTypeInfo().IsGenericType &&
                     (pluggedType.GetTypeInfo().BaseType.GetGenericTypeDefinition() == templateType))
            {
                yield return pluggedType.GetTypeInfo().BaseType;
            }

            if (pluggedType.GetTypeInfo().BaseType == typeof(object)) yield break;

            foreach (var interfaceType in FindInterfacesThatClosesCore(pluggedType.GetTypeInfo().BaseType, templateType))
            {
                yield return interfaceType;
            }
        }

        private static bool IsConcrete(this Type type)
        {
            return !type.GetTypeInfo().IsAbstract && !type.GetTypeInfo().IsInterface;
        }

        private static void Fill<T>(this IList<T> list, T value)
        {
            if (list.Contains(value)) return;
            list.Add(value);
        }

        public static void AddRequiredServices(IServiceCollection services, MediatorServiceConfiguration serviceConfiguration)
        {
            // Use TryAdd, so any existing ServiceFactory/IMediator registration doesn't get overriden
            // Cannot use TryAdd to register a delegate with lifetime in runtime
            // This line:
            // services.TryAdd(new ServiceDescriptor(typeof(ServiceFactory), p => p.GetService, serviceConfiguration.Lifetime));
            // cause a compile error
            // so I have to use switch instead
            // TODO: Replace switch by TryAdd if supported
            switch (serviceConfiguration.Lifetime)
            {
                case ServiceLifetime.Singleton:
                    services.TryAddSingleton<ServiceFactory>(p => p.GetService);
                    break;
                case ServiceLifetime.Scoped:
                    services.TryAddScoped<ServiceFactory>(p => p.GetService);
                    break;
                case ServiceLifetime.Transient:
                    services.TryAddTransient<ServiceFactory>(p => p.GetService);
                    break;
            }
            
            services.TryAdd(new ServiceDescriptor(typeof(NovelWorld.Mediator.IMediator), serviceConfiguration.MediatorImplementationType, serviceConfiguration.Lifetime));
            services.TryAdd(new ServiceDescriptor(typeof(MediatR.IMediator), sp => sp.GetService<NovelWorld.Mediator.IMediator>(), serviceConfiguration.Lifetime));
            services.TryAdd(new ServiceDescriptor(typeof(ISender), sp => sp.GetService<IMediator>(), serviceConfiguration.Lifetime));
            services.TryAdd(new ServiceDescriptor(typeof(IPublisher), sp => sp.GetService<IMediator>(), serviceConfiguration.Lifetime));

            // Use TryAddTransientExact (see below), we do want to register our Pre/Post processor behavior, even if (a more concrete)
            // registration for IPipelineBehavior<,> already exists. But only once.
            services.TryAddExact(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>), serviceConfiguration.HandlerLifetime);
            services.TryAddExact(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>), serviceConfiguration.HandlerLifetime);
            services.TryAddExact(typeof(IPipelineBehavior<,>), typeof(RequestExceptionActionProcessorBehavior<,>), serviceConfiguration.HandlerLifetime);
            services.TryAddExact(typeof(IPipelineBehavior<,>), typeof(RequestExceptionProcessorBehavior<,>), serviceConfiguration.HandlerLifetime);
        }

        /// <summary>
        /// Adds a new transient registration to the service collection only when no existing registration of the same service type and implementation type exists.
        /// In contrast to TryAddTransient, which only checks the service type.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="serviceType">Service type</param>
        /// <param name="implementationType">Implementation type</param>
        /// <param name="serviceLifetime">DI service lifetime</param>
        private static void TryAddExact(this IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime serviceLifetime)
        {
            if (services.Any(reg => reg.ServiceType == serviceType && reg.ImplementationType == implementationType))
            {
                return;
            }

            services.Add(new ServiceDescriptor(serviceType, implementationType, serviceLifetime));
        }
    }
}