using NovelWorld.Domain.Configurations;

namespace NovelWorld.Identity.Domain.Configurations
{
    public class IdentityAppSettings: AppSettings
    {
        public IdentityServerConfiguration IdentityServerConfiguration { get; set; }
    }
}