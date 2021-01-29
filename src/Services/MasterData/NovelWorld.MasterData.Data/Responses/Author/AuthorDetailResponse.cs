using System.Collections.Generic;
using NovelWorld.Data.Responses;

namespace NovelWorld.MasterData.Data.Responses
{
    public class AuthorDetailResponse: EntityResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        
        public IEnumerable<BookGeneralResponse> Books { get; set; }
    }
}