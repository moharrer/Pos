using Inventory.Api.Application.Events;
using Inventory.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Inventory.Api.Configuration
{
    public class UnitOfWorkActionFilter : IAsyncActionFilter
    {
        private InventoryContext inventoryContext;
        private IInventoryEventService paymentEventService;

        public UnitOfWorkActionFilter(InventoryContext paymentContext, IInventoryEventService inventoryEventService)
        {
            this.inventoryContext = paymentContext;
            this.paymentEventService = inventoryEventService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //paymentContext = context.HttpContext.RequestServices<PaymentContext>

            var strategy = inventoryContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
             {
                 using (var transaction = await inventoryContext.BeginTransactionAsync())
                 {

                     var resultContext = await next();

                     await inventoryContext.CommitTransactionAsync(transaction);

                     await paymentEventService.PublishEventsThroughEventBusAsync(transaction.TransactionId);
                 }
             });
        }
    }
}
