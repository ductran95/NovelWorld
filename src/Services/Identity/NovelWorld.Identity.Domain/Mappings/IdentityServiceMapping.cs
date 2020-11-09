using System;
using IdentityServer4.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NovelWorld.EventBus;
using NovelWorld.Identity.Domain.Queries.Abstractions;
using NovelWorld.Identity.Domain.Queries.Implements;
using NovelWorld.Identity.Domain.Services.Implements;
using NovelWorld.Identity.Infrastructure.Contexts;
using NovelWorld.Identity.Infrastructure.Repositories.Abstracts;
using NovelWorld.Identity.Infrastructure.Repositories.Implements;
using NovelWorld.Identity.Infrastructure.UoW.Implements;
using NovelWorld.Infrastructure.EntityFrameworkCore.Contexts;
using NovelWorld.Infrastructure.EntityFrameworkCore.Repositories.Implements;
using NovelWorld.Infrastructure.EntityFrameworkCore.UoW.Implements;
using NovelWorld.Infrastructure.Repositories.Abstractions;
using NovelWorld.Infrastructure.UoW.Abstractions;

namespace NovelWorld.Identity.Domain.Mappings
{
    public static class IdentityServiceMapping
    {
        public static IServiceCollection RegisterServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterContexts(configuration)
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

        private static IServiceCollection RegisterContexts(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
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