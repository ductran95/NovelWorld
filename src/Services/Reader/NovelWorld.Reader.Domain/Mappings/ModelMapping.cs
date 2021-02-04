using System;
using AutoMapper;
using NovelWorld.Reader.Data.Entities;
using NovelWorld.Reader.Data.Responses.BookComment;
using NovelWorld.Reader.Domain.Commands.BookComment;

namespace NovelWorld.Reader.Domain.Mappings
{
    public class ModelMapping : Profile
    {
        public ModelMapping()
        {
            #region BookComment

            CreateMap<BookComment, BookCommentResponse>();
            CreateMap<BookComment, BookCommentTreeViewResponse>();
            CreateMap<CreateBookCommentRequest, BookComment>();
            CreateMap<UpdateBookCommentRequest, BookComment>();
            CreateMap<Guid, DeleteBookCommentRequest>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));

            #endregion
        }
    }
}