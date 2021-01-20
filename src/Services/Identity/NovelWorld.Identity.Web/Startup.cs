using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
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
using NovelWorld.API.Filters;
using NovelWorld.API.Mappings;
using NovelWorld.Utility.Extensions;
using NovelWorld.Data.Constants;
using NovelWorld.Domain.Mappings;
using NovelWorld.EventBus.Extensions;
using NovelWorld.Identity.Web.Certificates;
using NovelWorld.Identity.Data.Configurations;
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
            services.AddControllersWithViews();
            
            // Get all Novel World assemblies
            var novelWorldAssemblies = AppDomain.CurrentDomain.GetAssemblies("NovelWorld");
            
            // Configuration
            var appSetting = new IdentityAppSettings();
            Configuration.Bind(appSetting);
            services.AddBaseAppConfig(Configuration).AddAppConfig(Configuration);
            
            // Add Mediatr
            services.AddScoped<Mediator.IMediator, CustomMediator>();
            services.AddScoped<MediatR.IMediator>(p => p.GetService<Mediator.IMediator>());
            services.AddMediatR(novelWorldAssemblies, configuration => configuration.Using<CustomMediator>().AsScoped().AsScopedHandler());
            services.RegisterDefaultPublishStrategies();
            services.RegisterDefaultProxies();

            // Add AutoMapper
            services.AddAutoMapper(novelWorldAssemblies);
            
            // Add Fluent Validation, Response filter
            services.AddScoped<SecurityHeadersAttribute>();
            services.AddScoped<RequestValidationFilter>();
            services.AddScoped<HttpSwitchModelResponseExceptionFilter>();
            services.AddValidatorsFromAssemblies(novelWorldAssemblies);
            services.AddMvc(options =>
                {
                    options.Filters.Add<RequestValidationFilter>();
                    options.Filters.Add<HttpSwitchModelResponseExceptionFilter>();
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
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });

            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;
            
            // Add Event Bus
            services.RegisterDefaultEventBus(appSetting.EventBusConfig);
            services.AddIntegrationEventHandler(new Type[]
            {
                typeof(IdentityModelMapping)
            });
            
            // Add DI
            services.RegisterDefaultHelpers();
            services.RegisterDefaultEventSourcing();
            services.RegisterAuthContext();
            services.RegisterServices();
            
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

            switch (appSetting.EventBusConfig.Type)
            {
                case EventBusTypes.RabbitMQ:
                    hcBuilder
                        .AddRabbitMQ(
                            $"amqp://{appSetting.EventBusConfig.EventBusConnection}",
                            name: "identity-rabbitmq-check",
                            tags: new string[] { "rabbitmq" });
                    break;
                    
                case EventBusTypes.AzureServiceBus:
                    hcBuilder
                        .AddAzureServiceBusTopic(
                            appSetting.EventBusConfig.EventBusConnection,
                            topicName: appSetting.EventBusConfig.SubscriptionClientName,
                            name: "identity-azureservicebus-check",
                            tags: new string[] { "azureservicebus" });
                    break;
            }
            
            // Add Identity Server
            services.AddIdentityServer()
                .AddInMemoryIdentityResources(appSetting.IdentityServerConfig.IdentityResources)
                .AddInMemoryApiResources(appSetting.IdentityServerConfig.ApiResources)
                .AddInMemoryApiScopes(appSetting.IdentityServerConfig.ApiScopes)
                .AddInMemoryClients(appSetting.IdentityServerConfig.Clients)
                .AddProfileService<ProfileService>()
                // .AddDeveloperSigningCredential();
                .AddSigningCredential(Certificate.Get(appSetting.IdentityServerConfig.CertificatePassword));

            // Add ADFS
            if (appSetting.IdentityServerConfig.OpenIdProviders != null && appSetting.IdentityServerConfig.OpenIdProviders.Any())
            {
                var authBuilder = services.AddAuthentication();
                foreach (var option in appSetting.IdentityServerConfig.OpenIdProviders)
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
                app.UseExceptionHandler("/Home/Error");
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