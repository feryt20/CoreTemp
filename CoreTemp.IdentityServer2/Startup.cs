using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreTemp.IdentityServer2.Data;
using CoreTemp.IdentityServer2.Models;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CoreTemp.IdentityServer2
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
            services.AddControllers();

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("TestConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            services.AddIdentityServer()
                .AddInMemoryIdentityResources(InMemoryConfig.GetIdentityResources())
                .AddInMemoryApiScopes(InMemoryConfig.ApiScopes())
                .AddInMemoryClients(InMemoryConfig.GetClients())
                .AddAspNetIdentity<ApplicationUser>()
                .AddDeveloperSigningCredential(); //not something we want to use in a production environment;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

           // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                      name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }


    }
}

/*

http://localhost:58249/connect/token
grant_type="password",
username="",
password="",
scope="api1",
client_id="employee"
client_secret="codemazesecret"
http://localhost:58249/api/Account/register
{
    "Username":"Username",
    "Email":"a@a.a",
    "FirstName":"FirstName",
    "LastName":"LastName",
    "Password":"Password!20",
    "ConfirmPassword":"Password!20"
} 

 */