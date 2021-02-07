using System.Collections.Generic;
using NovelWorld.Data.Responses;
using NovelWorld.MasterData.Data.Responses.Book;

namespace NovelWorld.MasterData.Data.Responses.Author
{
    public class AuthorDetailResponse: EntityResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        
        public IEnumerable<BookGeneralResponse> Books { get; set; }
    }
}