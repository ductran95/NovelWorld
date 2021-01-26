using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NovelWorld.ConnectionProvider.Mappings;
using NovelWorld.ConnectionProvider.PostgreSql.Mappings;
using NovelWorld.Domain.Mappings;
using NovelWorld.EventBus;
using NovelWorld.Infrastructure.EntityFrameworkCore.Contexts;
using NovelWorld.Infrastructure.Mappings;
using NovelWorld.Infrastructure.UoW.Abstractions;
using NovelWorld.MasterData.Domain.Configurations;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.MasterData.Infrastructure.UoW.Implements;
using NovelWorld.Utility.Mappings;

namespace NovelWorld.MasterData.Domain.Mappings
{
    public static class ServiceMapping
    {
        public static IServiceCollection RegisterAppConfig(
            this IServiceCollection services, IConfiguration config)
        {
            services.RegisterDefaultAppConfig(config);
            
            services.Configure<MasterDataAppSettings>(config);

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

        private static IServiceCollection RegisterDbContexts(this IServiceCollection services)
        {
            services.AddDbContext<MasterDataDbContext>((sp, options) =>
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                options.UseNpgsql(sp.GetService<DbConnection>());
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            });

            services.AddScoped<EfCoreEntityDbContext>(sp => sp.GetService<MasterDataDbContext>());
            services.AddScoped<DbContext>(sp => sp.GetService<MasterDataDbContext>());

            return services;
        }
        
        private static IServiceCollection RegisterUoW(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, MasterDataUnitOfWork>();

            return services;
        }

        #endregion
    }
}