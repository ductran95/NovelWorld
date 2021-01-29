using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using System.Text.Json;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NovelWorld.API.Attributes;
using NovelWorld.API.Filters;
using NovelWorld.API.Formatters;
using NovelWorld.API.Mappings;
using NovelWorld.Authentication.Configurations;
using NovelWorld.Data.Constants;
using NovelWorld.Domain.Mappings;
using NovelWorld.EventBus.Configurations;
using NovelWorld.EventBus.Extensions;
using NovelWorld.MasterData.Domain.Configurations;
using NovelWorld.MasterData.Domain.Mappings;
using NovelWorld.Mediator;
using NovelWorld.Mediator.DependencyInjection;
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
            // Get all Novel World assemblies
            var novelWorldAssemblies = AppDomain.CurrentDomain.GetAssemblies("NovelWorld");
            
            // Configuration
            var appSetting = new AppSettings();
            Configuration.Bind(appSetting);
            services.RegisterAppConfig(Configuration);
            
            // Add Mediatr
            services.AddMediatR(novelWorldAssemblies, configuration =>
            {
                configuration.Using<CustomMediator>().AsScoped().AsScopedHandler();
            });
            services.RegisterDefaultMediator();

            // Add AutoMapper
            services.AddAutoMapper(novelWorldAssemblies);
            
            // Add Fluent Validation, Response filter
            services.AddScoped<SecurityHeadersAttribute>();
            services.AddScoped<RequestValidationAttribute>();
            services.AddScoped<DelegateUserOnAllowAnonymousAttribute>();
            services.AddValidatorsFromAssemblies(novelWorldAssemblies);
            services.AddControllers(options =>
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
            
            // Authentication
            JwtSecurityTokenHandler.DefaultInboundClaimFilter.Clear();
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = appSetting.OAuth2Configuration.Issuer;
                    options.Audience = appSetting.OAuth2Configuration.Audience;
                    options.RequireHttpsMetadata = appSetting.OAuth2Configuration.RequireHttps;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidAudience = appSetting.OAuth2Configuration.Audience,
                        ValidateIssuer = true,
                        ValidIssuer = appSetting.OAuth2Configuration.Issuer,
                        ValidateLifetime = true,
                        NameClaimType = AdditionalClaimTypes.Account
                    };
                });
            
            // Authorization
            services.AddAuthorization();
            
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
            
            // Swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo {Title = "NovelWorld Master Data API", Version = "v1"});
                options.TagActionsBy(api => new List<string>
                {
                    !string.IsNullOrEmpty(api.GroupName)
                        ? api.GroupName
                        : api.ActionDescriptor.RouteValues["controller"]
                });
                options.OrderActionsBy(api => $"{api.GroupName}{api.ActionDescriptor.RouteValues["controller"]}");
                options.DocInclusionPredicate((_, _) => true);
                options.CustomSchemaIds(type => type.FullName);
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{appSetting.OAuth2Configuration.Issuer}/connect/authorize"),
                            TokenUrl = new Uri($"{appSetting.OAuth2Configuration.Issuer}/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                {appSetting.OAuth2Configuration.ApiScope, "NovelWorld API"}
                            }
                        }
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                
                options.OperationFilter<SwaggerResponseOperationFilter>(appSetting.OAuth2Configuration.ApiScope);
            });
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
                app.UseApiExceptionHandler();
            }

            // app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseForwardedHeaders();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            // ReSharper disable once PossibleNullReferenceException
            var oauthConfig = app.ApplicationServices.GetService<IOptions<OAuth2Configuration>>().Value;
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("../swagger/v1/swagger.json", "NovelWorld Master Data API v1");
                options.OAuthClientId(oauthConfig.SwaggerClientId);
                options.OAuthClientSecret(oauthConfig.SwaggerClientSecret);
                options.OAuthAppName(oauthConfig.SwaggerAppName);
                options.OAuthRealm(oauthConfig.SwaggerRealm);
            });
            
            // Subscribe Integrate Event
            app.ApplicationServices.SubscribeIntegrationEvents();
        }
    }
}