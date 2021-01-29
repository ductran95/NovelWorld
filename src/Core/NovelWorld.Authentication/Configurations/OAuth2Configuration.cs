namespace NovelWorld.Authentication.Configurations
{
    public class OAuth2Configuration
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string ApiScope { get; set; }
        public bool RequireHttps { get; set; }
        public string SwaggerClientId { get; set; }
        public string SwaggerClientSecret { get; set; }
        public string SwaggerRealm { get; set; }
        public string SwaggerAppName { get; set; }
    }
}