using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Payment.Api.Application.Events;
using Payment.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Api.Configuration
{
    public class UnitOfWorkActionFilter : IAsyncActionFilter
    {
        private PaymentContext paymentContext;
        private IPaymentEventService paymentEventService;

        public UnitOfWorkActionFilter(PaymentContext paymentContext, IPaymentEventService paymentEventService)
        {
            this.paymentContext = paymentContext;
            this.paymentEventService = paymentEventService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //paymentContext = context.HttpContext.RequestServices<PaymentContext>

            var strategy = paymentContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
             {
                 using (var transaction = await paymentContext.BeginTransactionAsync())
                 {

                     var resultContext = await next();

                     await paymentContext.CommitTransactionAsync(transaction);

                     await paymentEventService.PublishEventsThroughEventBusAsync(transaction.TransactionId);
                 }
             });
        }
    }
}
