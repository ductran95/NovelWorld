using AutoMapper;
using NovelWorld.Authentication.DTO;
using NovelWorld.Identity.Data.Entities;

namespace NovelWorld.Identity.Domain.Mappings
{
    public class IdentityModelMapping : Profile
    {
        public IdentityModelMapping()
        {
            #region User

            CreateMap<User, AuthenticatedUser>().ReverseMap();

            #endregion
        }
    }
}
