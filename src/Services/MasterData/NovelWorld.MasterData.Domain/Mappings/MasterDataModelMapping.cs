using System;
using AutoMapper;
using NovelWorld.MasterData.Data.Entities;
using NovelWorld.MasterData.Data.Responses;
using NovelWorld.MasterData.Domain.Commands;

namespace NovelWorld.MasterData.Domain.Mappings
{
    public class MasterDataModelMapping : Profile
    {
        public MasterDataModelMapping()
        {
            #region Author

            CreateMap<Author, AuthorGeneralResponse>();
            CreateMap<Author, AuthorDetailResponse>();
            CreateMap<CreateAuthorCommand, Author>();
            CreateMap<UpdateAuthorCommand, Author>();
            CreateMap<Guid, DeleteAuthorCommand>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));

            #endregion
            
            #region Book

            CreateMap<Book, BookGeneralResponse>();
            CreateMap<Book, BookDetailResponse>();
            CreateMap<CreateBookCommand, Book>();
            CreateMap<UpdateBookCommand, Book>();
            CreateMap<Guid, DeleteBookCommand>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));

            #endregion
            
            #region Category

            CreateMap<Category, CategoryGeneralResponse>();
            CreateMap<Category, CategoryDetailResponse>();
            CreateMap<CreateCategoryCommand, Category>();
            CreateMap<UpdateCategoryCommand, Category>();
            CreateMap<Guid, DeleteCategoryCommand>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));

            #endregion
            
            #region Chapter

            CreateMap<Chapter, ChapterGeneralResponse>();
            CreateMap<Chapter, ChapterDetailResponse>();
            CreateMap<CreateChapterCommand, Chapter>();
            CreateMap<UpdateChapterCommand, Chapter>();
            CreateMap<Guid, DeleteChapterCommand>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));

            #endregion
        }
    }
}