using System;
using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NovelWorld.EventBus;
using NovelWorld.Identity.Data.Configurations;
using NovelWorld.Identity.Domain.Queries.Abstractions;
using NovelWorld.Identity.Domain.Queries.Implements;
using NovelWorld.Identity.Infrastructure.Contexts;
using NovelWorld.Identity.Infrastructure.Repositories.Abstracts;
using NovelWorld.Identity.Infrastructure.Repositories.Implements;
using NovelWorld.Identity.Infrastructure.UoW.Implements;
using NovelWorld.Infrastructure.UoW.Abstractions;

namespace NovelWorld.Identity.Domain.Mappings
{
    public static class IdentityServiceMapping
    {
        public static IServiceCollection AddAppConfig(
            this IServiceCollection services, IConfiguration config)
        {
            services.Configure<IdentityAppSettings>(config);
            services.Configure<IdentityServerConfig>(config.GetSection(nameof(IdentityServerConfig)));

            return services;
        }
        
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services
                .RegisterContexts()
                .RegisterUoW()
                .RegisterRepositories()
                .RegisterQueries();

            return services;
        }

        public static IServiceProvider SubscribeIntegrationEvents(this IServiceProvider serviceProvider)
        {
            var eventBus = serviceProvider.GetRequiredService<IEventBus>();


            return serviceProvider;
        }

        #region Private

        private static IServiceCollection RegisterQueries(
            this IServiceCollection services)
        {
            #region User

            services.AddScoped<IUserQuery, UserQuery>();

            #endregion

            return services;
        }

        private static IServiceCollection RegisterRepositories(
            this IServiceCollection services)
        {
            #region User

            services.AddScoped<IUserRepository, UserRepository>();

            #endregion

            return services;
        }

        private static IServiceCollection RegisterContexts(this IServiceCollection services)
        {
            services.AddDbContext<IdentityContext>((sp, options) =>
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                options.UseNpgsql(sp.GetService<DbConnection>());
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            });

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