using NovelWorld.EventBus.Configurations;
using NovelWorld.Storage.Configurations;

namespace NovelWorld.Identity.Domain.Configurations
{
    public class AppSettings
    {
        public EventBusConfiguration EventBusConfiguration { get; set; }
        public StorageConfiguration StorageConfiguration { get; set; }
        public IdentityServerConfiguration IdentityServerConfiguration { get; set; }
        
        public string AllowedHosts { get; set; }
        public string[] AllowedOrigins { get; set; }
    }
}