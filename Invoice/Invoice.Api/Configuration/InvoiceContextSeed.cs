using Invoice.Api.Configuration;
using Invoice.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using System;
using System.Threading.Tasks;

namespace Inventory.Api.Configuration
{
    public class InvoiceContextSeed
    {
        public async Task SeedAsync(InvoiceContext context, IWebHostEnvironment env, IOptions<InvoiceSettings> settings, ILogger<InvoiceContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(InvoiceContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                var contentRootPath = env.ContentRootPath;


                using (context)
                {
                    context.Database.Migrate();
                }

                return Task.CompletedTask;
            });
        }


        private AsyncRetryPolicy CreatePolicy(ILogger<InvoiceContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
                    }
                );
        }
    }
}
