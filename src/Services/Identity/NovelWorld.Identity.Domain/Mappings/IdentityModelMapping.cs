using AutoMapper;
using NovelWorld.Authentication.DTO;
using NovelWorld.Identity.Data.Entities;
using NovelWorld.Identity.Data.ViewModels.Account;
using NovelWorld.Identity.Domain.Commands.User;

namespace NovelWorld.Identity.Domain.Mappings
{
    public class IdentityModelMapping : Profile
    {
        public IdentityModelMapping()
        {
            #region User

            CreateMap<RegisterInputModel, RegisterUserCommand>();
            CreateMap<RegisterUserCommand, User>();
            CreateMap<User, AuthenticatedUser>().ReverseMap();

            #endregion
        }
    }
}
