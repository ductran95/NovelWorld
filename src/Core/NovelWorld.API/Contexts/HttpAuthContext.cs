using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NovelWorld.Authentication.Contexts.Implements;
using NovelWorld.Authentication.DTO;
using NovelWorld.Authentication.Exceptions;
using NovelWorld.Data.Constants;

namespace NovelWorld.API.Contexts
{
    public class HttpAuthContext : IAuthContext
    {
        private readonly IHttpContextAccessor _contextAccessor;
        
        private AuthenticatedUser _user;
        private readonly IPAddress _ip;

        public AuthenticatedUser User
        {
            get
            {
                if (_user == null)
                {
                    throw new UnauthenticatedException();
                }

                return _user;
            }
        }

        public IPAddress IP => _ip;

        public HttpAuthContext(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            
            var context = contextAccessor.HttpContext;
            var contextUser = context.User;
            var claims = contextUser?.Claims;
            if (contextUser != null && claims != null && claims.Any())
            {
                var idStr = contextUser.FindFirstValue(AdditionalClaimTypes.UserId);
                var nameStr = contextUser.FindFirstValue(AdditionalClaimTypes.Account);
                var fullNameStr = contextUser.FindFirstValue(AdditionalClaimTypes.UserFullName);
                var emailStr = contextUser.FindFirstValue(AdditionalClaimTypes.UserEmail);
                var rolesStr = contextUser.FindFirstValue(AdditionalClaimTypes.UserRoles);

                if (!string.IsNullOrEmpty(idStr))
                {
                    _user = new AuthenticatedUser()
                    {
                        // ReSharper disable once AssignNullToNotNullAttribute
                        Id =  Guid.Parse(idStr),
                        Account = nameStr,
                        FullName = fullNameStr,
                        Email = emailStr,
                        Roles = !string.IsNullOrEmpty(rolesStr) ? JsonSerializer.Deserialize<IEnumerable<string>>(rolesStr) : new List<string>()
                    };
                }
            }

            _ip = context.Connection.RemoteIpAddress;
        }

        internal void SetDefaultUser()
        {
            if (_user == null)
            {
                var context = _contextAccessor.HttpContext;
                var configuration = context.RequestServices.GetRequiredService<IConfiguration>();
                _user = configuration.GetSection("DefaultUser").Get<AuthenticatedUser>();
            }
        }
    }
}