using System;
using System.Linq;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
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
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Commands;
using NovelWorld.Domain.Mappings;
using NovelWorld.EventBus.Extensions;
using NovelWorld.Identity.API.Mappings;
using NovelWorld.Identity.Data.Configurations;
using NovelWorld.Identity.Domain.Mappings;
using NovelWorld.Mediator;

namespace NovelWorld.Identity.API
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
            
            // Configuration
            var appSetting = new IdentityAppSettings();
            Configuration.Bind(appSetting);
            services.AddBaseAppConfig(Configuration).AddAppConfig(Configuration);
            
            // Add Mediatr
            services.AddTransient<Mediator.IMediator, CustomMediator>();
            services.AddTransient<MediatR.IMediator>(p => p.GetService<Mediator.IMediator>());
            services.AddMediatR(new Type[]
            {
                typeof(BaseModelMapping),
                typeof(IdentityModelMapping)
            }, configuration => configuration.Using<CustomMediator>());
            services.RegisterPublishStrategies();
            services.RegisterBaseProxies();

            // Add AutoMapper
            services.AddAutoMapper(typeof(BaseModelMapping));
            services.AddAutoMapper(typeof(IdentityModelMapping));
            
            // Add Fluent Validation
            services.AddScoped<RequestValidationFilter>();
            services.AddScoped<HttpResponseExceptionFilter>();
            services.AddValidatorsFromAssemblyContaining(typeof(Request)); // Data.Core project
            services.AddValidatorsFromAssemblyContaining(typeof(Command)); // Domain.Core project
            //services.AddValidatorsFromAssemblyContaining(typeof()); // Domain project
            services.AddMvc(options =>
                {
                    options.Filters.Add<RequestValidationFilter>();
                    options.Filters.Add<HttpResponseExceptionFilter>();
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
                    //options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;
            
            // Add Event Bus
            services.RegisterBaseEventBus(appSetting.EventBusConfig);
            services.AddIntegrationEventHandler(new Type[]
            {
                typeof(IdentityModelMapping)
            });
            
            // Add DI
            services.RegisterAuthContext();
            services.RegisterBaseEventSourcing();
            services.RegisterServices();
            
            // Config CORS
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));
            
            // Add Filter
            services.AddScoped<SecurityHeadersAttribute>();
            
            // Add HealthCheck
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy());
            
            // Add Identity Server
            services.AddIdentityServer()
                .AddInMemoryIdentityResources(appSetting.IdentityServerConfig.IdentityResources)
                .AddInMemoryApiResources(appSetting.IdentityServerConfig.ApiResources)
                .AddInMemoryApiScopes(appSetting.IdentityServerConfig.ApiScopes)
                .AddInMemoryClients(appSetting.IdentityServerConfig.Clients)
                .AddDeveloperSigningCredential();
                // .AddSigningCredential(Certificate.Get());

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

            app.UseAuthorization();
            
            app.UseForwardedHeaders();

            app.UseCors("CorsPolicy");
            
            // Adds IdentityServer
            app.UseIdentityServer();
            app.UseAuthorization();

            // Fix a problem with chrome. Chrome enabled a new feature "Cookies without SameSite must be secure", 
            // the cookies shold be expided from https, but in akaLink, the internal comunicacion in aks and docker compose is http.
            // To avoid this problem, the policy of cookies shold be in Lax mode.
            app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.None });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHealthChecks("/health");
            });
            
            // Subscribe Integrate Event
            // app.SubscribeIntegrationEvents();
        }
    }
}