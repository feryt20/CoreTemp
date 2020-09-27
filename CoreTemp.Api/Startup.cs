using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Threading.Tasks;
using CoreTemp.Api.Helpers.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CoreTemp.Services.Seed;
using Microsoft.AspNetCore.Http;
using ZNetCS.AspNetCore.Logging.EntityFrameworkCore;
using CoreTemp.Data.Models.Log;
using CoreTemp.Data.DatabaseContext;

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
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                      name: "pay",
                    pattern: "{controller=PG}/{action=pay}/{id?}");
            });
        }
    }
}
