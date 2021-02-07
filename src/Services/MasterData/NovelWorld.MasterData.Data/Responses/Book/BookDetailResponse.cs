using System;
using System.Collections.Generic;
using NovelWorld.Data.Responses;
using NovelWorld.MasterData.Data.Enums;
using NovelWorld.MasterData.Data.Responses.Author;
using NovelWorld.MasterData.Data.Responses.Category;

namespace NovelWorld.MasterData.Data.Responses.Book
{
    public class BookDetailResponse: EntityResponse
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