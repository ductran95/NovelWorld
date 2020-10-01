using System;

namespace NovelWorld.Data.Responses
{
    public class LookupResponse: Response
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}