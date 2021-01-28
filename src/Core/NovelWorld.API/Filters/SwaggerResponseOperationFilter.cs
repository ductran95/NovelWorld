using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NovelWorld.API.Filters
{
    public class SwaggerResponseOperationFilter : IOperationFilter
    {
        private readonly string _apiScope;
        public SwaggerResponseOperationFilter(string apiScope)
        {
            _apiScope = apiScope;
        }
        
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check for authorize attribute
            var hasAuthorize = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                               context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            if (!hasAuthorize) return;

            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthenticated" });
            operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Unauthorized" });

            var oauth2Scheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "oauth2"}
            };

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    [oauth2Scheme] = new[] { _apiScope }
                }
            };
        }
    }
}
