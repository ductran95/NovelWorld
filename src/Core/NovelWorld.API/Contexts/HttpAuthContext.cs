using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using NovelWorld.Authentication.Contexts;
using NovelWorld.Authentication.Contexts.Implements;
using NovelWorld.Authentication.DTO;
using NovelWorld.Authentication.Exceptions;
using NovelWorld.Data.Constants;

namespace NovelWorld.API.Contexts
{
    public class HttpAuthContext : IAuthContext
    {
        private readonly AuthenticatedUser _user;
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
            var context = contextAccessor.HttpContext;
            var contextUser = context.User;
            var claims = contextUser?.Claims;
            if (contextUser != null && claims != null && claims.Any())
            {
                var idStr = contextUser.FindFirstValue(AdditionalClaims.UserId);
                var nameStr = contextUser.FindFirstValue(AdditionalClaims.UserName);
                var fullNameStr = contextUser.FindFirstValue(AdditionalClaims.UserFullName);
                var emailStr = contextUser.FindFirstValue(AdditionalClaims.UserEmail);
                var rolesStr = contextUser.FindFirstValue(AdditionalClaims.UserRoles);

                if (string.IsNullOrEmpty(idStr))
                {
                    _user = new AuthenticatedUser()
                    {
                        // ReSharper disable once AssignNullToNotNullAttribute
                        Id =  Guid.Parse(idStr),
                        UserName = nameStr,
                        FullName = fullNameStr,
                        Email = emailStr,
                        Roles = !string.IsNullOrEmpty(rolesStr) ? JsonSerializer.Deserialize<IEnumerable<string>>(rolesStr) : new List<string>()
                    };
                }
            }

            _ip = context.Connection.RemoteIpAddress;
        }
    }
}