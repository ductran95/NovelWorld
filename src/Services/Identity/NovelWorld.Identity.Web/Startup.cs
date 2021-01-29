using System;
using System.Linq;
using System.Text.Json;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using NovelWorld.API.Attributes;
using NovelWorld.API.Formatters;
using NovelWorld.API.Mappings;
using NovelWorld.Utility.Extensions;
using NovelWorld.Domain.Mappings;
using NovelWorld.EventBus.Configurations;
using NovelWorld.EventBus.Extensions;
using NovelWorld.Identity.Web.Certificates;
using NovelWorld.Identity.Domain.Configurations;
using NovelWorld.Identity.Domain.Mappings;
using NovelWorld.Identity.Web.Extensions;
using NovelWorld.Identity.Web.Services.Implements;
using NovelWorld.Mediator;
using NovelWorld.Mediator.DependencyInjection;

namespace NovelWorld.Identity.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Get all Novel World assemblies
            var novelWorldAssemblies = AppDomain.CurrentDomain.GetAssemblies("NovelWorld");
            
            // Configuration
            var appSetting = new AppSettings();
            Configuration.Bind(appSetting);
            services.RegisterAppConfig(Configuration);
            
            // Add Mediatr
            services.AddMediatR(novelWorldAssemblies, configuration => configuration.Using<CustomMediator>().AsScoped().AsScopedHandler());
            services.RegisterDefaultMediator();

            // Add AutoMapper
            services.AddAutoMapper(novelWorldAssemblies);
            
            // Add Fluent Validation, Response filter
            services.AddScoped<SecurityHeadersAttribute>();
            services.AddScoped<DelegateUserOnAllowAnonymousAttribute>();
            services.AddValidatorsFromAssemblies(novelWorldAssemblies);
            services
                .AddMvc(options =>
                {
                    options.RespectBrowserAcceptHeader = true;
                    options.InputFormatters.Add(new TextPlainInputFormatter());
                })
                .AddFluentValidation(fv =>
                {
                    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    fv.ImplicitlyValidateChildProperties = true;
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                })
                .AddJsonOptions(options =>
                {
                    // options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });

            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;
            
            // Add Event Bus
            services.RegisterIntegrationEventHandler(novelWorldAssemblies);
            
            // Add DI
            services.RegisterHttpAuthContext();
            services.RegisterServices(appSetting);
            
            // Config CORS
            var allowedOrigin = Configuration.GetValue<string[]>("AllowedOrigins");
            if (allowedOrigin == null)
            {
                allowedOrigin = new string[0];
            }

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins(allowedOrigin)
                    .WithExposedHeaders("Content-Disposition");
            }));
            
            // Add HealthCheck
            var hcBuilder = services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddNpgSql(Configuration.GetConnectionString("DefaultConnection"));

            switch (appSetting.EventBusConfiguration.Type)
            {
                case EventBusTypes.RabbitMQ:
                    hcBuilder
                        .AddRabbitMQ(
                            $"amqp://{appSetting.EventBusConfiguration.Connection}",
                            name: "identity-rabbitmq-check",
                            tags: new string[] { "rabbitmq" });
                    break;
                    
                case EventBusTypes.AzureServiceBus:
                    hcBuilder
                        .AddAzureServiceBusTopic(
                            appSetting.EventBusConfiguration.Connection,
                            topicName: appSetting.EventBusConfiguration.SubscriptionClientName,
                            name: "identity-azureservicebus-check",
                            tags: new string[] { "azureservicebus" });
                    break;
            }
            
            // Add Identity Server
            services
                .AddIdentityServer(options =>
                {
                    options.UserInteraction.ErrorUrl = "/Error";
                })
                .AddInMemoryIdentityResources(appSetting.IdentityServerConfiguration.IdentityResources)
                .AddInMemoryApiResources(appSetting.IdentityServerConfiguration.ApiResources)
                .AddInMemoryApiScopes(appSetting.IdentityServerConfiguration.ApiScopes)
                .AddInMemoryClients(appSetting.IdentityServerConfiguration.Clients)
                .AddProfileService<ProfileService>()
                // .AddDeveloperSigningCredential();
                .AddSigningCredential(Certificate.Get(appSetting.IdentityServerConfiguration.CertificatePassword));

            // Add ADFS
            if (appSetting.IdentityServerConfiguration.OpenIdProviders != null && appSetting.IdentityServerConfiguration.OpenIdProviders.Any())
            {
                var authBuilder = services.AddAuthentication();
                foreach (var option in appSetting.IdentityServerConfiguration.OpenIdProviders)
                {
                    authBuilder.AddOpenIdConnect(option.AuthenticationScheme, option.DisplayName, o => o.SetOpenIdConnectOptions(option));
                }
            }

            IdentityModelEventSource.ShowPII = true; //To show detail of error and see the problem
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseMvcExceptionHandler();
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseForwardedHeaders();
            app.UseCors("CorsPolicy");
            
            // Adds IdentityServer
            app.UseIdentityServer();
            app.UseAuthorization();

            // Fix a problem with chrome. Chrome enabled a new feature "Cookies without SameSite must be secure", 
            // the cookies should be expired from https, but in akaLink, the internal communication in aks and docker compose is http.
            // To avoid this problem, the policy of cookies should be in Lax mode.
            app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.None });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHealthChecks("/health");
            });
            
            // Subscribe Integrate Event
            app.ApplicationServices.SubscribeIntegrationEvents();
        }
    }
}