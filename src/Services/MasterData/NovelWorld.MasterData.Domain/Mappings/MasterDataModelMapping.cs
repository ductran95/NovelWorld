using System;
using AutoMapper;
using NovelWorld.MasterData.Data.Entities;
using NovelWorld.MasterData.Data.Responses;
using NovelWorld.MasterData.Domain.Commands;
using NovelWorld.MasterData.Domain.Commands.Author;

namespace NovelWorld.MasterData.Domain.Mappings
{
    public class MasterDataModelMapping : Profile
    {
        public MasterDataModelMapping()
        {
            #region Author

            CreateMap<Author, AuthorGeneralResponse>();
            CreateMap<Author, AuthorDetailResponse>();
            CreateMap<CreateAuthorRequest, Author>();
            CreateMap<UpdateAuthorRequest, Author>();
            CreateMap<Guid, DeleteAuthorRequest>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));

            #endregion
        }
    }
}