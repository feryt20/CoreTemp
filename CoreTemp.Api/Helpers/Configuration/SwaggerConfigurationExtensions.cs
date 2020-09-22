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
                document.DocumentName = "v1_Api";
                document.ApiGroupNames = new[] { "v1_Api" };
                document.PostProcess = d =>
                {
                    d.Info.Title = "V1 Api Docs For Payment Section";
                };
                document.AddSecurity("Bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Description = "Copy this into the value field: Bearer {token}",
                    Scheme = "Bearer",
                    In = OpenApiSecurityApiKeyLocation.Header
                });

                document.OperationProcessors.Add(
                    new AspNetCoreOperationSecurityScopeProcessor("Bearer"));

            });

        }

        public static void UsePaySwagger(this IApplicationBuilder app)
        {
            app.UseOpenApi();
            app.UseSwaggerUi3(); // serve Swagger UI
        }

    }
}
