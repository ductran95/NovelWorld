using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts;
using NovelWorld.Mediator;
using NovelWorld.Common.Extensions;

namespace NovelWorld.Domain.Proxies
{
    public class LoggingProxy<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        #region Properties
        protected readonly ILogger<LoggingProxy<TRequest, TResponse>> _logger;
        protected readonly IAuthContext _authContext;
        #endregion

        #region Constructor
        public LoggingProxy(ILogger<LoggingProxy<TRequest, TResponse>> logger, IAuthContext authContext)
        {
            this._logger = logger;
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
                _logger.LogInformation("----- Handling command {CommandName} from user {User} and IP {IP} with data ({@Command})", requestName, user, ip, request);
                var response = await next();
                _logger.LogInformation("----- Command {CommandName} handled - response: {@Response}", requestName, response);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Handling command {CommandName} ({@Command})", requestName, request);
                throw;
            }
        }
    }

    public class LoggingProxy<TNotification>: INotificationPipelineBehavior<TNotification>
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
                _logger.LogInformation("----- Handling event {CommandName} from user {User} and IP {IP} with data ({@Command})", requestName, user, ip, request);
                await next();
                _logger.LogInformation("----- Event {CommandName} handled", requestName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Handling event {CommandName} ({@Command})", requestName, request);
                // Do not throw exception when handling event failed
            }
        }
    }
}
