using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NovelWorld.Domain.Extensions;

namespace NovelWorld.Domain.Proxies
{
    public sealed class AuthorizeProxy<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        #region Properties
        private readonly IServiceProvider _services;
        #endregion

        #region Constructor
        public AuthorizeProxy(IServiceProvider services)
        {
            _services = services;
        }
        #endregion

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestHandler = _services.GetService<IRequestHandler<TRequest, TResponse>>();

            var authAttribute = requestHandler.GetAuthorizeAttribute();

            if(authAttribute != null)
            {
            }

            return await next();
        }
    }
}
