namespace NovelWorld.Data.Configurations
{
    public class OAuth2Config
    {
        public string Audience { get; set; }
        public bool RequireHttps { get; set; }
        public string SwaggerClientId { get; set; }
        public string SwaggerClientSecret { get; set; }
        public string SwaggerRealm { get; set; }
        public string SwaggerAppName { get; set; }
    }
}