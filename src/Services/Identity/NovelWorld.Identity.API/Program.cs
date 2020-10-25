using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace NovelWorld.Identity.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = GetConfiguration();
            CreateHostBuilder(config, args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(IConfiguration configuration, string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
                .UseSerilog((builderContext, config) =>
                {
                    config.ReadFrom.Configuration(configuration);
                });

        private static IConfiguration GetConfiguration() =>
            new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("identityserver.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
    }
}