using CoreTemp.Repo.Infrastructure;
using CoreTemp.Services.Seed;
using CoreTemp.Services.Sms;
using CoreTemp.Services.Upload;
using CoreTemp.Services.Utility;
using DnsClient;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTemp.Api.Helpers.Configuration
{
    public static class DIConfigurationExtensions
    {
        public static void AddPayDI(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddTransient<SeedService>();

            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

            services.AddScoped<IUtilities, Utilities>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IUploadService, UploadService>();

            services.AddSingleton<ILookupClient, LookupClient>();
        }
    }
}
