using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NovelWorld.Authentication.Configurations;
using NovelWorld.ConnectionProvider.Configurations;
using NovelWorld.ConnectionProvider.Mappings;
using NovelWorld.ConnectionProvider.PostgreSql.Mappings;
using NovelWorld.EventBus.AzureServiceBus.Mappings;
using NovelWorld.EventBus.Bus.Abstractions;
using NovelWorld.EventBus.Configurations;
using NovelWorld.EventBus.Mappings;
using NovelWorld.EventBus.RabbitMQ.Mappings;
using NovelWorld.Infrastructure.EntityFrameworkCore.Contexts;
using NovelWorld.Infrastructure.Mappings;
using NovelWorld.Infrastructure.UoW.Abstractions;
using NovelWorld.Reader.Domain.Configurations;
using NovelWorld.Reader.Infrastructure.Contexts;
using NovelWorld.Reader.Infrastructure.UoW.Implements;
using NovelWorld.Utility.Mappings;

namespace NovelWorld.Reader.Domain.Mappings
{
    public static class ServiceMapping
    {
        public static IServiceCollection RegisterAppConfig(
            this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AppSettings>(config);
            services.Configure<DbConfiguration>(config.GetSection(nameof(DbConfiguration)));
            services.Configure<OAuth2Configuration>(config.GetSection(nameof(OAuth2Configuration)));
            services.Configure<EventBusConfiguration>(config.GetSection(nameof(EventBusConfiguration)));

            return services;
        }
        
        public static IServiceCollection RegisterServices(this IServiceCollection services, AppSettings appSettings)
        {
            services
                .RegisterDefaultHelpers()
                .RegisterDefaultEventSourcing()
                .RegisterDbContexts(appSettings.DbConfiguration)
                .RegisterUoW()
                .RegisterEventBus(appSettings.EventBusConfiguration)
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
                    services.RegisterRabbitMQ();
                    break;
        
                case EventBusTypes.AzureServiceBus:
                    services.RegisterAzureServiceBus();
                    break;
        
                default:
                    services.RegisterDefaultEventBus();
                    break;
            }
        
            return services;
        }
        
        private static IServiceCollection RegisterDbContexts(this IServiceCollection services, DbConfiguration dbConfiguration)
        {
            switch (dbConfiguration.Type)
            {
                case DbTypes.PostgreSql:
                    services
                        .RegisterPostgreSqlDbConnectionFactory()
                        .AddDbContext<ReaderDbContext>((sp, options) =>
                        {
                            // ReSharper disable once AssignNullToNotNullAttribute
                            options.UseNpgsql(sp.GetService<DbConnection>());
                            options.EnableDetailedErrors();
                            options.EnableSensitiveDataLogging();
                        });
                    break;
                
            }
            
            services.RegisterDefaultDbConnection();
            services.AddScoped<EfCoreEntityDbContext>(sp => sp.GetService<ReaderDbContext>());
            services.AddScoped<DbContext>(sp => sp.GetService<ReaderDbContext>());

            return services;
        }
        
        private static IServiceCollection RegisterUoW(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, ReaderUnitOfWork>();

            return services;
        }

        #endregion
    }
}