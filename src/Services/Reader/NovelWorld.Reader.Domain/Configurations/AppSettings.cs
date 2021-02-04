using NovelWorld.Authentication.Configurations;
using NovelWorld.ConnectionProvider.Configurations;
using NovelWorld.EventBus.Configurations;

namespace NovelWorld.Reader.Domain.Configurations
{
    public class AppSettings
    {
        public DbConfiguration DbConfiguration { get; set; }
        public OAuth2Configuration OAuth2Configuration { get; set; }
        public EventBusConfiguration EventBusConfiguration { get; set; }
        public string AllowedHosts { get; set; }
        public string[] AllowedOrigins { get; set; }
        
        public string ConnectionString { get; set; }
    }
}