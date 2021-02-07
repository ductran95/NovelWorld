using System;
using NovelWorld.Data.Responses;

namespace NovelWorld.Reader.Data.Responses.BookComment
{
    public class BookCommentResponse: EntityResponse
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public string Content { get; set; }
        
        public Guid? ParentId { get; set; }
    }
}