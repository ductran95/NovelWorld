namespace NovelWorld.Data.Configurations
{
    public class AppSettings
    {
        // public EnvironmentEnum Environment { get; set; }
        // public UrlConfig UrlConfig { get; set; }
        public AttachmentConfig AttachmentConfig { get; set; }
        public OAuth2Config OAuth2Config { get; set; }
        public EventBusConfig EventBusConfig { get; set; }
        public string AllowedHosts { get; set; }
        public string[] AllowedOrigins { get; set; }
    }
}