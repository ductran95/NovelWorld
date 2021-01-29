using NovelWorld.Authentication.Configurations;
using NovelWorld.EventBus.Configurations;
using NovelWorld.Storage.Configurations;

namespace NovelWorld.MasterData.Domain.Configurations
{
    public class AppSettings
    {
        public OAuth2Configuration OAuth2Configuration { get; set; }
        public EventBusConfiguration EventBusConfiguration { get; set; }
        public StorageConfiguration StorageConfiguration { get; set; }
        public string AllowedHosts { get; set; }
        public string[] AllowedOrigins { get; set; }
    }
}