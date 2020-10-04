namespace NovelWorld.Infrastructure.EventSourcing
{
    public class DbChangedEvent
    {
        public string Name { get; set; }
        public string Action { get; set; }
        public object Data { get; set; }
    }
}