using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NovelWorld.Authentication.Configurations;
using NovelWorld.ConnectionProvider.Mappings;
using NovelWorld.ConnectionProvider.PostgreSql.Mappings;
using NovelWorld.EventBus;
using NovelWorld.EventBus.AzureServiceBus.Mappings;
using NovelWorld.EventBus.Configurations;
using NovelWorld.EventBus.Mappings;
using NovelWorld.EventBus.RabbitMQ.Mappings;
using NovelWorld.Identity.Domain.Configurations;
using NovelWorld.Identity.Infrastructure.Contexts;
using NovelWorld.Identity.Infrastructure.UoW.Implements;
using NovelWorld.Infrastructure.EntityFrameworkCore.Contexts;
using NovelWorld.Infrastructure.Mappings;
using NovelWorld.Infrastructure.UoW.Abstractions;
using NovelWorld.Storage.Configurations;
using NovelWorld.Utility.Mappings;

namespace NovelWorld.Identity.Domain.Mappings
{
    public static class ServiceMapping
    {
        public static IServiceCollection RegisterAppConfig(
            this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AppSettings>(config);
            services.Configure<OAuth2Configuration>(config.GetSection(nameof(OAuth2Configuration)));
            services.Configure<EventBusConfiguration>(config.GetSection(nameof(EventBusConfiguration)));
            services.Configure<StorageConfiguration>(config.GetSection(nameof(StorageConfiguration)));
            services.Configure<IdentityServerConfiguration>(config.GetSection(nameof(IdentityServerConfiguration)));

            return services;
        }
        
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration config)
        {
            services
                .RegisterDefaultHelpers()
                .RegisterPostgreSqlDbConnectionFactory(config.GetConnectionString("DefaultConnection"))
                .RegisterDefaultDbConnection()
                .RegisterDefaultEventSourcing()
                .RegisterDbContexts()
                .RegisterUoW()
                .RegisterEventBus(new EventBusConfiguration())
                .RegisterQueries()
                .RegisterCommands();

            return services;
        }

        public static IServiceProvider SubscribeIntegrationEvents(this IServiceProvider serviceProvider)
        {
            var eventBus = serviceProvider.GetRequiredService<IEventBus>();


            return serviceProvider;
        }

        #region Private

        private static IServiceCollection RegisterCommands(
            this IServiceCollection services)
        {
            

            return services;
        }
        
        private static IServiceCollection RegisterQueries(
            this IServiceCollection services)
        {
            

            return services;
        }
        
        private static IServiceCollection RegisterEventBus(this IServiceCollection services, EventBusConfiguration eventBusConfig)
        {
            services.RegisterDefaultEventBusSubscriptionsManager();
        
            switch (eventBusConfig.Type)
            {
                case EventBusTypes.RabbitMQ:
                    services.RegisterRabbitMQ(eventBusConfig);
        
                    break;
        
                 case EventBusTypes.AzureServiceBus:
                     services.RegisterAzureServiceBus(eventBusConfig);
        
                    break;
        
                default:
                    services.RegisterDefaultEventBus();
                    break;
            }
        
            return services;
        }
        
        private static IServiceCollection RegisterDbContexts(this IServiceCollection services)
        {
            services.AddDbContext<IdentityDbContext>((sp, options) =>
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                options.UseNpgsql(sp.GetService<DbConnection>());
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            });
            
            services.AddScoped<EfCoreEntityDbContext>(sp => sp.GetService<IdentityDbContext>());
            services.AddScoped<DbContext>(sp => sp.GetService<IdentityDbContext>());

            return services;
        }
        
        private static IServiceCollection RegisterUoW(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, IdentityUnitOfWork>();

            return services;
        }

        #endregion
    }
}