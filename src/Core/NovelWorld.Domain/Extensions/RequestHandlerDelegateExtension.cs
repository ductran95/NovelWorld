using System.Reflection;
using MediatR;
using NovelWorld.Domain.Attributes;

namespace NovelWorld.Domain.Extensions
{
    public static class RequestHandlerDelegateExtension
    {
        public static UnitOfWorkAttribute GetUnitOfWorkAttribute<TRequest, TResponse>(this IRequestHandler<TRequest, TResponse> handler) where TRequest: IRequest<TResponse>
        {
            var uow = handler.GetType().GetCustomAttribute<UnitOfWorkAttribute>();

            // Unit of Work by default
            // If no [UnitOfWork]
            if (uow == null)
            {
                uow = new UnitOfWorkAttribute(true);
            }

            return uow;
        }
        
        public static AuthorizeAttribute GetAuthorizeAttribute<TRequest, TResponse>(this IRequestHandler<TRequest, TResponse> handler) where TRequest: IRequest<TResponse>
        {
            var authorizeAttribute = handler.GetType().GetCustomAttribute<AuthorizeAttribute>();

            return authorizeAttribute;
        }
    }
}
