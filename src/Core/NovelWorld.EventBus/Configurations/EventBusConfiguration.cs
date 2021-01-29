namespace NovelWorld.EventBus.Configurations
{
    public class EventBusConfiguration
    {
        public string Type { get; set; }
        public string SubscriptionClientName { get; set; }
        public string Connection { get; set; }
        public int RetryCount { get; set; } = 5;
    }
}