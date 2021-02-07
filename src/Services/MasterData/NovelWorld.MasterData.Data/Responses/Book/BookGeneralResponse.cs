using System;
using NovelWorld.Data.Responses;
using NovelWorld.MasterData.Data.Enums;

namespace NovelWorld.MasterData.Data.Responses.Book
{
    public class BookGeneralResponse: EntityResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public BookStatusEnum Status { get; set; }
        public float Rate { get; set; }
        public string Cover { get; set; }
        public Guid AuthorId { get; set; }
    }
}