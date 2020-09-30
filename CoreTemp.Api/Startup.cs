using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CoreTemp.Api.Helpers.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CoreTemp.Services.Seed;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using CoreTemp.Api.Helpers;

namespace CoreTemp.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPayDbContext(Configuration);
            services.AddPayInitialize();
            services.AddPayIdentityInit();
            services.AddPayAuth(Configuration);

            services.AddAutoMapper(typeof(Startup));

            services.AddPayDI();

            services.AddPayApiVersioning();
            services.AddPaySwagger();
            services.AddMadParbad(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SeedService seed)
        {
            app.UsePayExceptionHandle(env);
            app.UsePayInitialize(seed);

            app.UsePayAuth();
            app.UsePaySwagger();
            app.UseMadParbad();

            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = new PathString("/wwwroot")
            });

            //var rewriteOptions = new RewriteOptions();
            //rewriteOptions.Rules.Add(new NonWwwRewriteRule());
            //app.UseRewriter(rewriteOptions);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                      name: "pay",
                    pattern: "{controller=PG}/{action=Pay}/{id?}");
            });
        }
    }
}
