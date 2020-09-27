using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.Generation.Processors.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTemp.Api.Helpers.Configuration
{
    public static class SwaggerConfigurationExtensions
    {
        public static void AddPaySwagger(this IServiceCollection services)
        {
            services.AddOpenApiDocument(document =>
            {
                document.DocumentName = "v1_Site";
                document.ApiGroupNames = new[] { "v1_Site" };
                document.PostProcess = d =>
                {
                    d.Info.Title = "V1 Api Docs For Site Section";
                };

                document.AddSecurity("Bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Description = "Copy this into the value field: Bearer {token}",
                    Scheme = "Bearer",
                    In = OpenApiSecurityApiKeyLocation.Header
                });
            });

            services.AddOpenApiDocument(document =>
            {
                document.DocumentName = "v1_User";
                document.ApiGroupNames = new[] { "v1_User" };
                document.PostProcess = d =>
                {
                    d.Info.Title = "V1 Api Docs For User Section";
                };

                document.AddSecurity("Bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Description = "Copy this into the value field: Bearer {token}",
                    Scheme = "Bearer",
                    In = OpenApiSecurityApiKeyLocation.Header
                });
            });

            services.AddOpenApiDocument(document =>
            {
                document.DocumentName = "v1_Admin";
                document.ApiGroupNames = new[] { "v1_Admin" };
                document.PostProcess = d =>
                {
                    d.Info.Title = "V1 Api Docs For Admin Section";
                };

                document.AddSecurity("Bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Description = "Copy this into the value field: Bearer {token}",
                    Scheme = "Bearer",
                    In = OpenApiSecurityApiKeyLocation.Header
                });
            });
        }

        public static void UsePaySwagger(this IApplicationBuilder app)
        {
            app.UseOpenApi();
            app.UseSwaggerUi3(); // serve Swagger UI
        }

    }
}
