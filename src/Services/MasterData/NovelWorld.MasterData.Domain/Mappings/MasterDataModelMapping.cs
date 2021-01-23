using System;
using AutoMapper;
using NovelWorld.MasterData.Data.Entities;
using NovelWorld.MasterData.Data.Responses;
using NovelWorld.MasterData.Domain.Commands.Author;
using NovelWorld.MasterData.Domain.Commands.Book;
using NovelWorld.MasterData.Domain.Commands.Category;
using NovelWorld.MasterData.Domain.Commands.Chapter;

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
            
            #region Category

            CreateMap<Category, CategoryGeneralResponse>();
            CreateMap<Category, CategoryDetailResponse>();
            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>();
            CreateMap<Guid, DeleteCategoryRequest>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));

            #endregion
            
            #region Book

            CreateMap<Book, BookGeneralResponse>();
            CreateMap<Book, BookDetailResponse>();
            CreateMap<CreateBookRequest, Book>();
            CreateMap<UpdateBookRequest, Book>();
            CreateMap<Guid, DeleteBookRequest>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));

            #endregion
            
            #region Chapter

            CreateMap<Chapter, ChapterGeneralResponse>();
            CreateMap<Chapter, ChapterDetailResponse>();
            CreateMap<CreateChapterRequest, Chapter>();
            CreateMap<UpdateChapterRequest, Chapter>();
            CreateMap<Guid, DeleteChapterRequest>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));

            #endregion
        }
    }
}