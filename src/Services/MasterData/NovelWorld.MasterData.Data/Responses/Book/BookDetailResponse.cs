using System;
using System.Collections.Generic;
using NovelWorld.MasterData.Data.Enums;

namespace NovelWorld.MasterData.Data.Responses
{
    public class BookDetailResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public BookStatusEnum Status { get; set; }
        public float Rate { get; set; }
        public string Cover { get; set; }
        public Guid AuthorId { get; set; }
        
        public AuthorGeneralResponse Author { get; set; }
        public IEnumerable<CategoryGeneralResponse> Categories { get; set; }
    }
}