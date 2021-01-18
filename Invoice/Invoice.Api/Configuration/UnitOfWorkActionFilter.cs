using Invoice.Api.Application.Events;
using Invoice.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Invoice.Api.Configuration
{
    public class UnitOfWorkActionFilter : IAsyncActionFilter
    {
        private InvoiceContext paymentContext;
        private IInvoiceEventService paymentEventService;

        public UnitOfWorkActionFilter(InvoiceContext paymentContext, IInvoiceEventService paymentEventService)
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
