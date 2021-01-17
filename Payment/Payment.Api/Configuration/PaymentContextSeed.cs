using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Payment.Data;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Api.Configuration
{
    public class PaymentContextSeed
    {
        public async Task SeedAsync(PaymentContext context, IWebHostEnvironment env, IOptions<PaymentSettings> settings, ILogger<PaymentContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(PaymentContextSeed));

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


        private AsyncRetryPolicy CreatePolicy(ILogger<PaymentContextSeed> logger, string prefix, int retries = 3)
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
