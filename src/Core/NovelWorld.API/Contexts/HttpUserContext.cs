using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NovelWorld.Authentication.Contexts;
using NovelWorld.Authentication.Exceptions;
using NovelWorld.Common.Constants;
using NovelWorld.Data.Entities.Auth;

namespace NovelWorld.API.Contexts
{
    public class HttpUserContext: IUserContext
    {
        private readonly User _user;

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

        public HttpUserContext(IHttpContextAccessor contextAccessor)
        {
            var context = contextAccessor.HttpContext.User;
            var claims = context?.Claims;
            if (context != null && claims != null && claims.Any())
            {
                _user = new User()
                {
                    Id = Guid.Parse(context.FindFirstValue(AdditionalClaims.UserId)),
                    UserName = context.FindFirstValue(AdditionalClaims.UserName),
                    FullName = context.FindFirstValue(AdditionalClaims.UserFullName),
                    Email = context.FindFirstValue(AdditionalClaims.UserEmail),
                };
            }
        }
    }
}