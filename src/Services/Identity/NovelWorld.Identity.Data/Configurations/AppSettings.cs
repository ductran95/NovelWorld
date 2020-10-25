using NovelWorld.Data.Configurations;

namespace NovelWorld.Identity.Data.Configurations
{
    public class IdentityAppSettings: AppSettings
    {
        public IdentityServerConfig IdentityServerConfig { get; set; }
    }
}