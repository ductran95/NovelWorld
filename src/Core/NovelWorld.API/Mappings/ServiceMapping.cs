using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using NovelWorld.API.Contexts.Implements;
using NovelWorld.API.Middlewares;
using NovelWorld.Authentication.Contexts.Abstractions;

namespace NovelWorld.API.Mappings
{
    public static class ServiceMapping
    {
        public static IServiceCollection RegisterHttpAuthContext(this IServiceCollection services)
        {
            services.TryAddScoped<IAuthContext, HttpAuthContext>();

            return services;
        }
        
        public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<ApiExceptionHandlerMiddleware>();
        }
        
        public static IApplicationBuilder UseMvcExceptionHandler(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<MvcExceptionHandlerMiddleware>();
        }
        
        public static IApplicationBuilder UseMvcExceptionHandler(this IApplicationBuilder app, MvcExceptionHandlerOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<MvcExceptionHandlerMiddleware>(Options.Create(options));
        }
        
        public static IApplicationBuilder UseApiAndMvcExceptionHandler(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<ApiAndMvcExceptionHandlerMiddleware>();
        }
        
        public static IApplicationBuilder UseApiAndMvcExceptionHandler(this IApplicationBuilder app, MvcExceptionHandlerOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<ApiAndMvcExceptionHandlerMiddleware>(Options.Create(options));
        }
    }
}
