using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace NovelWorld.Mediator
{
    public interface IPublisher
    {
        Task Publish(IEnumerable<Func<INotification, CancellationToken, Task>> handlers, INotification notification, IServiceScope scope, CancellationToken cancellationToken);
        public IServiceScope GetServiceScope();
        ServiceFactory GetServiceFactory(IServiceScope scope);
        public void DisposeScope(IServiceScope scope);
    }

    public class PublisherParallelWhenAll : IPublisher
    {
        private readonly IServiceProvider _services;
        public PublisherParallelWhenAll(IServiceProvider services)
        {
            _services = services;
        }

        public IServiceScope GetServiceScope()
        {
            return null;
        }

        public ServiceFactory GetServiceFactory(IServiceScope scope)
        {
            return _services.GetService;
        }

        public Task Publish(IEnumerable<Func<INotification, CancellationToken, Task>> handlers, INotification notification, IServiceScope scope, CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();

            foreach (var handler in handlers)
            {
                tasks.Add(Task.Run(() => handler(notification, cancellationToken)));
            }

            return Task.WhenAll(tasks);
        }

        public void DisposeScope(IServiceScope scope)
        {
        }
    }

    public class PublisherParallelWhenAny : IPublisher
    {
        private readonly IServiceProvider _services;
        public PublisherParallelWhenAny(IServiceProvider services)
        {
            _services = services;
        }

        public IServiceScope GetServiceScope()
        {
            return null;
        }

        public ServiceFactory GetServiceFactory(IServiceScope scope)
        {
            return _services.GetService;
        }

        public Task Publish(IEnumerable<Func<INotification, CancellationToken, Task>> handlers, INotification notification, IServiceScope scope, CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();

            foreach (var handler in handlers)
            {
                tasks.Add(Task.Run(() => handler(notification, cancellationToken)));
            }

            return Task.WhenAny(tasks);
        }

        public void DisposeScope(IServiceScope scope)
        {
        }
    }

    public class PublisherParallelNoWait : IPublisher
    {
        private readonly IServiceProvider _services;
        public PublisherParallelNoWait(IServiceProvider services)
        {
            _services = services;
        }

        public IServiceScope GetServiceScope()
        {
            return _services.CreateScope();
        }

        public ServiceFactory GetServiceFactory(IServiceScope scope)
        {
            return scope.ServiceProvider.GetService;
        }

        public Task Publish(IEnumerable<Func<INotification, CancellationToken, Task>> handlers, INotification notification, IServiceScope scope, CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();

            foreach (var handler in handlers)
            {
                tasks.Add(Task.Run(() => handler(notification, cancellationToken)));
            }

            Task.WhenAll(tasks).ContinueWith((task) => DisposeScope(scope));

            return Task.CompletedTask;
        }

        public void DisposeScope(IServiceScope scope)
        {
            if (scope != null)
            {
                scope.Dispose();
            }
        }
    }

    public class PublisherAsyncContinueOnException : IPublisher
    {
        private readonly IServiceProvider _services;
        public PublisherAsyncContinueOnException(IServiceProvider services)
        {
            _services = services;
        }

        public IServiceScope GetServiceScope()
        {
            return null;
        }

        public ServiceFactory GetServiceFactory(IServiceScope scope)
        {
            return _services.GetService;
        }

        public async Task Publish(IEnumerable<Func<INotification, CancellationToken, Task>> handlers, INotification notification, IServiceScope scope, CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();
            var exceptions = new List<Exception>();

            foreach (var handler in handlers)
            {
                try
                {
                    tasks.Add(handler(notification, cancellationToken));
                }
                catch (Exception ex) when (!(ex is OutOfMemoryException || ex is StackOverflowException))
                {
                    exceptions.Add(ex);
                }
            }

            try
            {
                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
            catch (AggregateException ex)
            {
                exceptions.AddRange(ex.Flatten().InnerExceptions);
            }
            catch (Exception ex) when (!(ex is OutOfMemoryException || ex is StackOverflowException))
            {
                exceptions.Add(ex);
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }

        public void DisposeScope(IServiceScope scope)
        {
        }
    }

    public class PublisherSyncStopOnException : IPublisher
    {
        private readonly IServiceProvider _services;
        public PublisherSyncStopOnException(IServiceProvider services)
        {
            _services = services;
        }

        public IServiceScope GetServiceScope()
        {
            return null;
        }

        public ServiceFactory GetServiceFactory(IServiceScope scope)
        {
            return _services.GetService;
        }

        public async Task Publish(IEnumerable<Func<INotification, CancellationToken, Task>> handlers, INotification notification, IServiceScope scope, CancellationToken cancellationToken)
        {
            foreach (var handler in handlers)
            {
                await handler(notification, cancellationToken).ConfigureAwait(false);
            }
        }

        public void DisposeScope(IServiceScope scope)
        {
        }
    }

    public class PublisherSyncContinueOnException : IPublisher
    {
        private readonly IServiceProvider _services;
        public PublisherSyncContinueOnException(IServiceProvider services)
        {
            _services = services;
        }

        public IServiceScope GetServiceScope()
        {
            return null;
        }

        public ServiceFactory GetServiceFactory(IServiceScope scope)
        {
            return _services.GetService;
        }

        public async Task Publish(IEnumerable<Func<INotification, CancellationToken, Task>> handlers, INotification notification, IServiceScope scope, CancellationToken cancellationToken)
        {
            var exceptions = new List<Exception>();

            foreach (var handler in handlers)
            {
                try
                {
                    await handler(notification, cancellationToken).ConfigureAwait(false);
                }
                catch (AggregateException ex)
                {
                    exceptions.AddRange(ex.Flatten().InnerExceptions);
                }
                catch (Exception ex) when (!(ex is OutOfMemoryException || ex is StackOverflowException))
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }

        public void DisposeScope(IServiceScope scope)
        {
        }
    }
}