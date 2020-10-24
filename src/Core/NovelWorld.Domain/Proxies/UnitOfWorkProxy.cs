using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NovelWorld.Domain.Extensions;
using NovelWorld.Infrastructure.EventSourcing;
using NovelWorld.Infrastructure.EventSourcing.Abstractions;
using NovelWorld.Infrastructure.UoW;

namespace NovelWorld.Domain.Proxies
{
    public class UnitOfWorkProxy<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest: IRequest<TResponse>
    {
        #region Properties
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IDbEventSource _dBEventSource;
        protected readonly IServiceProvider _services;
        #endregion

        #region Constructor
        public UnitOfWorkProxy(IServiceProvider services, IUnitOfWork unitOfWork, IDbEventSource dBEventSource)
        {
            _unitOfWork = unitOfWork;
            _dBEventSource = dBEventSource;
            _services = services;
        }
        #endregion

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            TResponse response;

            var requestHandler = _services.GetService<IRequestHandler<TRequest, TResponse>>();

            var uowAttribute = requestHandler.GetUnitOfWorkAttribute();

            // If executing code called from another block that create transaction
            if (_unitOfWork.IsInTransaction())
            {
                return await next();
            }

            // If not transactional
            if (!uowAttribute.IsTransaction)
            {
                response = await next();
                //await _unitOfWork.SaveChangesAsync();

                // Publish all DB Event
                PublishEvent();

                return response;
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                        
                response = await next();
                //await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();

                // Publish all DB Event
                PublishEvent();
            }
            catch(Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            return response;
        }


        public void PublishEvent()
        {
            var scope = _services.CreateScope();
            var newDbEventSource = scope.ServiceProvider.GetService<IDbEventSource>();

            // Transfer event list from old scope to new scope
            newDbEventSource.EventList = _dBEventSource.EventList;

            var publishTask = Task.Run(() => newDbEventSource.Publish());
            publishTask.ContinueWith((task) => scope.Dispose());
        }
    }
}
