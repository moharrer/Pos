using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventLogStore.Ef;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Payment.Api.Configuration;
using Payment.Data;

namespace Payment.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            host.MigrateDbContext<PaymentContext>((context, services) =>
            {
                var env = services.GetService<IWebHostEnvironment>();
                var settings = services.GetService<IOptions<PaymentSettings>>();
                var logger = services.GetService<ILogger<PaymentContextSeed>>();

                new PaymentContextSeed()
                    .SeedAsync(context, env, settings, logger)
                    .Wait();
            })
                .MigrateDbContext<EventLogDataContext>((_, __) => { });

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
