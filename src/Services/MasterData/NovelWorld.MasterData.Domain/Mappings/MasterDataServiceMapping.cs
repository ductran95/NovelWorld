using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NovelWorld.EventBus;
using NovelWorld.Infrastructure.UoW.Abstractions;
using NovelWorld.MasterData.Data.Configurations;
using NovelWorld.MasterData.Domain.Queries.Abstractions;
using NovelWorld.MasterData.Domain.Queries.Implements;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.MasterData.Infrastructure.Repositories.Abstracts;
using NovelWorld.MasterData.Infrastructure.Repositories.Implements;
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
        
        public static IServiceCollection RegisterServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            services
                .RegisterContexts(configuration)
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
            services.AddScoped<IAuthorQuery, AuthorQuery>();
            services.AddScoped<ICategoryQuery, CategoryQuery>();
            services.AddScoped<IBookQuery, BookQuery>();
            services.AddScoped<IChapterQuery, ChapterQuery>();

            return services;
        }

        private static IServiceCollection RegisterRepositories(
            this IServiceCollection services)
        {
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IChapterRepository, ChapterRepository>();

            return services;
        }

        private static IServiceCollection RegisterContexts(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MasterDataContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            });

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