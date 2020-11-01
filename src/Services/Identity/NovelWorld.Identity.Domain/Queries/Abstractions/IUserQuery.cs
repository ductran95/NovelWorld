using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using NovelWorld.Authentication.DTO;
using NovelWorld.Domain.Queries.Abstractions;

namespace NovelWorld.Identity.Domain.Queries.Abstractions
{
    public interface IUserQuery: IQuery
    {
        Task<bool> ValidateCredentials(string email, string password);
        Task<AuthenticatedUser> FindByEmail(string email);
        Task<AuthenticatedUser> FindByExternalProvider(string provider, string providerUserId);
    }
}