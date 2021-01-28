namespace NovelWorld.EventBus
{
    public class EventBusConfiguration
    {
        public string Type { get; set; }
        public string SubscriptionClientName { get; set; }
        public string EventBusConnection { get; set; }
        public string EventBusUserName { get; set; }
        public string EventBusPassword { get; set; }
        public int EventBusRetryCount { get; set; }
    }
}