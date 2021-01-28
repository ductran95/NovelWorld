using NovelWorld.Data.Responses;

namespace NovelWorld.MasterData.Data.Responses
{
    public class AuthorGeneralResponse: EntityResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}