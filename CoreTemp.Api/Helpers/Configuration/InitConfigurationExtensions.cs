using CoreTemp.Data.DatabaseContext;
using CoreTemp.Services.Seed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTemp.Api.Helpers.Configuration
{
    public static class InitConfigurationExtensions
    {
        public static void AddPayDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var con = configuration.GetSection("ConnectionStrings");

            services.AddDbContext<CoreTempDbContext>(opt => {
                opt.UseSqlServer(con.GetSection("Main").Value);
            });
            services.AddDbContext<LogDbContext>(opt => {
                opt.UseSqlServer(con.GetSection("Log").Value);
            });
            services.AddDbContext<BasketDbContext>(opt => {
                opt.UseSqlServer(con.GetSection("Basket").Value);
            });
        }
        public static void AddPayInitialize(this IServiceCollection services)
        {
            services.AddControllersWithViews();

            //services.AddMvc(opt =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //    .RequireAuthenticatedUser().Build();
            //    opt.Filters.Add(new AuthorizeFilter(policy));
            //});
            //services.AddCors(opt => opt.AddPolicy("CorsPolicy", builder =>
            //    builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials()));


            services.AddMvcCore(config =>
            {
                //config.ReturnHttpNotAcceptable = true;
                config.Filters.Add(typeof(RequireHttpsAttribute));
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            })
             .AddApiExplorer()
             .AddFormatterMappings()
             .AddDataAnnotations()
             .AddCors(opt =>
             {
                 opt.AddPolicy("CorsPolicy", builder =>
                 builder.WithOrigins("https://qazvinbuy.ir", "https://localhost:44345", "http://localhost:65444", "http://localhost:4200")
                         .AllowAnyMethod()
                         .AllowAnyHeader()
                         .AllowCredentials());
             })
             .AddNewtonsoftJson(opt =>
             {
                 opt.SerializerSettings.ReferenceLoopHandling =
                 Newtonsoft.Json.ReferenceLoopHandling.Ignore;
             });


            services.AddHsts(opt =>
            {
                opt.MaxAge = TimeSpan.FromDays(180);
                opt.IncludeSubDomains = true;
                opt.Preload = true;
            });

            services.AddHttpsRedirection(opt =>
            {
                opt.RedirectStatusCode = StatusCodes.Status301MovedPermanently;
            });


            services.AddResponseCaching();
            services.AddResponseCompression(opt => opt.Providers.Add<GzipCompressionProvider>());

            services.AddRouting(opt => opt.LowercaseUrls = true);

        }


        public static void UsePayInitialize(this IApplicationBuilder app, SeedService seeder)
        {
            app.UseResponseCompression();
            seeder.SeedUsers();
            app.UseRouting();

            app.UseCsp(opt => opt
           .StyleSources(s => s.Self()
           .UnsafeInline().CustomSources("qazvinbuy.ir", "fonts.googleapis.com"))
           .ScriptSources(s => s.Self()
           .UnsafeInline().UnsafeEval().CustomSources("qazvinbuy.ir", "apis.google.com"))
           .ImageSources(s => s.Self()
           .CustomSources("qazvinbuy.ir", "res.cloudinary.com", "data:"))
           .MediaSources(s => s.Self()
           .CustomSources("qazvinbuy.ir", "res.cloudinary.com", "cloudinary.com", "data:"))
           .FontSources(s => s.Self()
           .CustomSources("fonts.gstatic.com", "data:"))
           .FrameSources(s => s.Self()
           .CustomSources("accounts.google.com"))
           );


            app.UseXfo(o => o.Deny());
        }

        public static void UsePayInitializeInProduction(this IApplicationBuilder app)
        {
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseResponseCaching();
        }
    }
}
