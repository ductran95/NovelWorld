using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NovelWorld.EventBus;
using NovelWorld.Infrastructure.EntityFrameworkCore.Contexts;
using NovelWorld.Infrastructure.UoW.Abstractions;
using NovelWorld.MasterData.Data.Configurations;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.MasterData.Infrastructure.UoW.Implements;

namespace NovelWorld.MasterData.Domain.Mappings
{
    public static class MasterDataServiceMapping
    {
        public static IServiceCollection AddAppConfig(
            this IServiceCollection services, IConfiguration config)
        {
            services.Configure<MasterDataAppSettings>(config);

            return services;
        }
        
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services
                .RegisterContexts()
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


        private static IServiceCollection RegisterContexts(this IServiceCollection services)
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