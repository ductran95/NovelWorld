using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using NovelWorld.API.Attributes;
using NovelWorld.API.Filters;
using NovelWorld.API.Mappings;
using NovelWorld.Data.Configurations;
using NovelWorld.Data.Constants;
using NovelWorld.Domain.Mappings;
using NovelWorld.EventBus.Extensions;
using NovelWorld.MasterData.Domain.Mappings;
using NovelWorld.Mediator;
using NovelWorld.Utility.Extensions;

namespace NovelWorld.MasterData.API
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
            services.AddControllers();
            
            // Get all Novel World assemblies
            var novelWorldAssemblies = AppDomain.CurrentDomain.GetAssemblies("NovelWorld");
            
            // Configuration
            var appSetting = new AppSettings();
            Configuration.Bind(appSetting);
            services.AddBaseAppConfig(Configuration).AddAppConfig(Configuration);
            
            // Add Mediatr
            services.AddTransient<Mediator.IMediator, CustomMediator>();
            services.AddTransient<MediatR.IMediator>(p => p.GetService<Mediator.IMediator>());
            services.AddMediatR(novelWorldAssemblies, configuration => configuration.Using<CustomMediator>());
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
                typeof(MasterDataModelMapping)
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseForwardedHeaders();
            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
            
            // Subscribe Integrate Event
            app.ApplicationServices.SubscribeIntegrationEvents();
        }
    }
}