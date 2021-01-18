using EventLogStore.Ef;
using Inventory.Api.Configuration;
using Invoice.Api.Configuration;
using Invoice.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Invoice.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            host.MigrateDbContext<InvoiceContext>((context, services) =>
            {
                var env = services.GetService<IWebHostEnvironment>();
                var settings = services.GetService<IOptions<InvoiceSettings>>();
                var logger = services.GetService<ILogger<InvoiceContextSeed>>();

                new InvoiceContextSeed()
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
