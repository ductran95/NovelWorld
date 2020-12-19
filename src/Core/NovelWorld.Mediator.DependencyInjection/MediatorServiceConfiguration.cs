using System;
using Microsoft.Extensions.DependencyInjection;

namespace NovelWorld.Mediator.DependencyInjection
{
    public class MediatorServiceConfiguration
    {
        public Type MediatorImplementationType { get; private set; }
        public ServiceLifetime Lifetime { get; private set; }
        public ServiceLifetime HandlerLifetime { get; private set; }

        public MediatorServiceConfiguration()
        {
            MediatorImplementationType = typeof(CustomMediator);
            Lifetime = ServiceLifetime.Transient;
            HandlerLifetime = ServiceLifetime.Transient;
        }

        public MediatorServiceConfiguration Using<TMediator>() where TMediator : IMediator
        {
            MediatorImplementationType = typeof(TMediator);
            return this;
        }

        public MediatorServiceConfiguration AsSingleton()
        {
            Lifetime = ServiceLifetime.Singleton;
            return this;
        }
        
        public MediatorServiceConfiguration AsSingletonHandler()
        {
            HandlerLifetime = ServiceLifetime.Singleton;
            return this;
        }

        public MediatorServiceConfiguration AsScoped()
        {
            Lifetime = ServiceLifetime.Scoped;
            return this;
        }
        
        public MediatorServiceConfiguration AsScopedHandler()
        {
            HandlerLifetime = ServiceLifetime.Scoped;
            return this;
        }

        public MediatorServiceConfiguration AsTransient()
        {
            Lifetime = ServiceLifetime.Transient;
            return this;
        }
        
        public MediatorServiceConfiguration AsTransientHandler()
        {
            HandlerLifetime = ServiceLifetime.Transient;
            return this;
        }
    }
}