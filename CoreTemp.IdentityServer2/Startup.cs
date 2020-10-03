using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreTemp.IdentityServer2.Data;
using CoreTemp.IdentityServer2.Models;
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
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("TestConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            services.AddIdentityServer()
                .AddInMemoryIdentityResources(InMemoryConfig.GetIdentityResources())
                .AddInMemoryClients(InMemoryConfig.GetClients())
                .AddAspNetIdentity<ApplicationUser>()
                .AddDeveloperSigningCredential(); //not something we want to use in a production environment;

            services.AddMvc();
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(options =>
            //{
            //    // base-address of your identityserver
            //    options.Authority = "http://localhost:58249/";

            //    // name of the API resource
            //    options.Audience = "openid";

            //    options.RequireHttpsMetadata = false;
            //});


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

            //app.UseAuthorization();

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
client_id="ro.angular"
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