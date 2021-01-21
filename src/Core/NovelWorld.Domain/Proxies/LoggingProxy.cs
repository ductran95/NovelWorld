using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Domain.Exceptions;
using NovelWorld.Mediator;
using NovelWorld.Utility.Extensions;

namespace NovelWorld.Domain.Proxies
{
    public sealed class LoggingProxy<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest: IRequest<TResponse>
    {
        #region Properties
        private readonly ILogger<LoggingProxy<TRequest, TResponse>> _logger;
        private readonly IAuthContext _authContext;
        #endregion

        #region Constructor
        public LoggingProxy(ILogger<LoggingProxy<TRequest, TResponse>> logger, IAuthContext authContext)
        {
            _logger = logger;
            _authContext = authContext;
        }
        #endregion

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestName = request.GetGenericTypeName();
            var user = _authContext.User.Id;
            var ip = _authContext.IP;

            try
            {
                _logger.LogInformation("----- Handling {CommandName} from user {User} and IP {IP}", requestName,
                    user, ip);
                _logger.LogDebug("----- With data: {@Command}", request);
                var response = await next();
                _logger.LogInformation("----- {CommandName} handled", requestName);
                _logger.LogDebug("----- With response: {@Response}", response);

                return response;
            }
            catch (ValidateException validateException)
            {
                _logger.LogError(validateException, validateException.Message, requestName);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Handling {CommandName}", requestName);
                if (request is ICanSwallowException {SwallowException: true})
                {
                    return default;
                }
                throw;
            }
        }
    }

    public class LoggingProxy<TNotification>: INotificationPipelineBehavior<TNotification> where TNotification: INotification
    {
        #region Properties
        protected readonly ILogger<LoggingProxy<TNotification>> _logger;
        protected readonly IAuthContext _authContext;
        #endregion

        #region Constructor
        public LoggingProxy(ILogger<LoggingProxy<TNotification>> logger, IAuthContext authContext)
        {
            this._logger = logger;
            _authContext = authContext;
        }
        #endregion
        public async Task Handle(TNotification request, CancellationToken cancellationToken, NotificationHandlerDelegate next)
        {
            var requestName = request.GetGenericTypeName();
            var user = _authContext.User.Id;
            var ip = _authContext.IP;

            try
            {
                _logger.LogInformation("----- Handling event {CommandName} from user {User} and IP {IP}", requestName, user, ip);
                _logger.LogDebug("----- Event data: {@Command}", request);
                await next();
                _logger.LogInformation("----- Event {CommandName} handled", requestName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Handling event {CommandName}", requestName);
                if (request is ICanSwallowException {SwallowException: true})
                {
                    return;
                }
                throw;
            }
        }
    }
}
