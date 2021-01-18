using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventLogStore.Ef;
using Inventory.Api.Configuration;
using Inventory.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Inventory.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            host.MigrateDbContext<InventoryContext>((context, services) =>
            {
                var env = services.GetService<IWebHostEnvironment>();
                var settings = services.GetService<IOptions<InventorySettings>>();
                var logger = services.GetService<ILogger<InventoryContextSeed>>();

                new InventoryContextSeed()
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
