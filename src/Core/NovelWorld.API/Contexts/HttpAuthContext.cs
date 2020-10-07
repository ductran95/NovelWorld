using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using NovelWorld.Authentication.Contexts;
using NovelWorld.Authentication.Exceptions;
using NovelWorld.Common.Constants;
using NovelWorld.Data.Entities.Auth;

namespace NovelWorld.API.Contexts
{
    public class HttpAuthContext : IAuthContext
    {
        private readonly User _user;
        private readonly IEnumerable<string> _roles;
        private readonly IPAddress _ip;

        public User User
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

        public IEnumerable<string> Roles
        {
            get
            {
                if (_user == null)
                {
                    throw new UnauthenticatedException();
                }

                return _roles;
            }
        }

        public IPAddress IP => _ip;

        public HttpAuthContext(IHttpContextAccessor contextAccessor)
        {
            var context = contextAccessor.HttpContext;
            var contextUser = context.User;
            var claims = contextUser?.Claims;
            if (contextUser != null && claims != null && claims.Any())
            {
                _user = new User()
                {
                    Id = Guid.Parse(contextUser.FindFirstValue(AdditionalClaims.UserId)),
                    UserName = contextUser.FindFirstValue(AdditionalClaims.UserName),
                    FullName = contextUser.FindFirstValue(AdditionalClaims.UserFullName),
                    Email = contextUser.FindFirstValue(AdditionalClaims.UserEmail),
                };

                _roles = JsonSerializer.Deserialize<IEnumerable<string>>(
                    contextUser.FindFirstValue(AdditionalClaims.UserId));
            }

            _ip = context.Connection.RemoteIpAddress;
        }
    }
}