using EventBus;
using Inventory.Api.Application.Events;
using Inventory.Data;
using Product.Api.Application;
using Share.IntegrationEvents.Inventory;
using Share.IntegrationEvents.Invoice;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Api.Application.Event
{
    public class StartInvoicePaymentEventHandler : IEventHandler<StartInvoicePaymentEvent>
    {
        private readonly IEventBus eventBus;
        private readonly IProductService productService;
        private readonly InventoryContext inventoryContext;
        private readonly IInventoryEventService inventoryEventService;
        private readonly IProductRepository invoiceRepository;

        public StartInvoicePaymentEventHandler(IEventBus eventBus, IProductService productService, InventoryContext invoiceContext, IInventoryEventService inventoryEventService, IProductRepository invoiceRepository)
        {
            this.eventBus = eventBus;
            this.productService = productService;
            this.inventoryContext = invoiceContext;
            this.inventoryEventService = inventoryEventService;
            this.invoiceRepository = invoiceRepository;
        }

        public async Task Handle(StartInvoicePaymentEvent @event)
        {
            try
            {
                var productReservedEvent = new InventoryItemBalancedEvent() { InvoiceId = @event.InvoiceId };

                var products = await productService.GetAllAsync();
                foreach (var item in @event.ItemLines)
                {
                    var product = products.SingleOrDefault(a => a.Id == item.ItemId);
                    
                    if (product.Quantity< item.Quantity)
                    {
                        throw new Exception("Insufficient product.");
                    }
                    product.Quantity -= item.Quantity;
                    productService.Update(product);
                }

                await invoiceRepository.UnitOfWrok.SaveChangesAsync();

                eventBus.Publish(productReservedEvent);

            }
            catch (Exception ex)
            {
                var failEvent = new InventoryItemBalancedFailed() { InvoiceId = @event.InvoiceId, Description =  ex.Message};

                eventBus.Publish(failEvent);
            }

        }

        


    }
}
