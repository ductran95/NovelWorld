using System.Collections.Generic;

namespace NovelWorld.MasterData.Data.Responses
{
    public class CategoryDetailResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        
        public IEnumerable<BookGeneralResponse> Books { get; set; }
    }
}