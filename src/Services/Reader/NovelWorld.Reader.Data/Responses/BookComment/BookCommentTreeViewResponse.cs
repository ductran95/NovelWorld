using System.Collections.Generic;

namespace NovelWorld.Reader.Data.Responses.BookComment
{
    public class BookCommentTreeViewResponse: BookCommentResponse
    {
        public List<BookCommentTreeViewResponse> Replies { get; set; }
    }
}