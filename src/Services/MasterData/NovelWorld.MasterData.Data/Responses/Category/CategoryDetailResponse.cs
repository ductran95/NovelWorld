using System.Collections.Generic;
using NovelWorld.Data.Responses;
using NovelWorld.MasterData.Data.Responses.Book;

namespace NovelWorld.MasterData.Data.Responses.Category
{
    public class CategoryDetailResponse: EntityResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        
        public IEnumerable<BookGeneralResponse> Books { get; set; }
    }
}