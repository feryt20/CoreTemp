using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Parbad.Builder;
using Parbad.Storage.EntityFrameworkCore.Builder;

namespace CoreTemp.Api.Helpers.Configuration
{
    public static class ParbadConfigurationExtensions
    {
        public static void AddMadParbad(this IServiceCollection services, IConfiguration configuration)
        {
            var con = configuration.GetSection("ConnectionStrings");
            /*
            services.AddParbad()
                .ConfigureGateways(gateWayes =>
                {
                    gateWayes
                    .AddMellat()
                    .WithAccounts(accs =>
                    {
                        accs.AddFromConfiguration(configuration.GetSection("MellatBank"));
                    });
                    gateWayes
                    .AddZarinPal()
                    .WithAccounts(accs =>
                    {
                        accs.AddFromConfiguration(configuration.GetSection("ZarinPalBank"));
                    });
                    gateWayes
                    .AddParbadVirtual()
                    .WithOptions(bld => bld.GatewayPath = "/MyVirtualGateway");
                })
                .ConfigureHttpContext(bld => bld.UseDefaultAspNetCore())
                .ConfigureStorage(builder =>
                {
                    builder.UseEfCore(options =>
                    {
                        // Using SQL Server
                        var assemblyName = typeof(Startup).Assembly.GetName().Name;
                        //options.ConfigureDbContext = db => db.UseSqlServer(con.GetSection("Financial").Value, sql => sql.MigrationsAssembly(assemblyName));

                        //// If you prefer to have a separate MigrationHistory table for Parbad, you can change the above line to this:
                        options.ConfigureDbContext = db => db.UseSqlServer(con.GetSection("Financial").Value, sql =>
                        {
                            sql.MigrationsAssembly(assemblyName);
                            sql.MigrationsHistoryTable("ParbadHistoryTable");
                        });

                        //options.DefaultSchema = "SCHEMA NAME"; // optional

                        //options.PaymentTableOptions.Name = "TABLE NAME"; // optional
                        //options.PaymentTableOptions.Schema = "SCHEMA NAME"; // optional

                        //options.TransactionTableOptions.Name = "TABLE NAME"; // optional
                        //options.TransactionTableOptions.Schema = "SCHEMA NAME"; // optional
                    });
                });
            */

            services.AddParbad()
                .ConfigureGateways(gateways =>
                {
                    gateways
                        .AddMellat()
                        .WithAccounts(accounts =>
                        {
                            accounts.AddInMemory(account =>
                            {
                                account.TerminalId = 123;
                                account.UserName = "MyId";
                                account.UserPassword = "MyPassword";
                            });
                        });

                    gateways
                        .AddParbadVirtual()
                        .WithOptions(options => options.GatewayPath = "/MyVirtualGateway");
                })
                .ConfigureHttpContext(builder => builder.UseDefaultAspNetCore())
                .ConfigureStorage(builder => builder.UseMemoryCache());
        }

        public static void UseMadParbad(this IApplicationBuilder app)
        {
            app.UseParbadVirtualGateway();

        }
    }
}
