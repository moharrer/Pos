using EventBus;
using Invoice.Api.Application.Events;
using Invoice.Data;
using Share.IntegrationEvents;
using Share.IntegrationEvents.Inventory;
using Share.IntegrationEvents.Invoice;
using Share.IntegrationEvents.Payment;
using System;
using System.Threading.Tasks;

namespace Invoice.Api.Application.Event
{
    public class InvoiceEventHandler : IEventHandler<RecordPaymentSubmitedEvent>, 
        IEventHandler<InventoryItemBalancedEvent>,
        IEventHandler<InventoryItemBalancedFailed>,
        IEventHandler<InvoiceFailedToPaiedEvent>
    {
        private readonly IEventBus eventBus;
        private readonly IInvoiceService invoiceService;
        private readonly InvoiceContext invoiceContext;
        private readonly IInvoiceEventService invoiceEventService;
        private readonly IInvoiceRepository invoiceRepository;

        public InvoiceEventHandler(IEventBus eventBus, IInvoiceService invoiceService, InvoiceContext invoiceContext, IInvoiceEventService invoiceEventService, IInvoiceRepository invoiceRepository)
        {
            this.eventBus = eventBus;
            this.invoiceService = invoiceService;
            this.invoiceContext = invoiceContext;
            this.invoiceEventService = invoiceEventService;
            this.invoiceRepository = invoiceRepository;
        }

        public async Task Handle(InventoryItemBalancedEvent @event)
        {
            var invoice = await invoiceService.GetByIdAsync(@event.InvoiceId);

            var recordPayment = new RequestPaymentEvent();

            recordPayment.InvoiceId = @event.InvoiceId;
            recordPayment.Amount = invoice.TotalAmount;

            eventBus.Publish(recordPayment);
        }

        public async Task Handle(InventoryItemBalancedFailed @event)
        {
            var invoice = await invoiceService.GetByIdAsync(@event.InvoiceId);
            
            /************************************************************/
            
            //Here we need take a decision how should we do with out of stock invoice

            /***********************************************************/
        }

        public async Task Handle(RecordPaymentSubmitedEvent @event)
        {
            try
            {
                //TODO: do some validation on @event
                await invoiceService.MarkAsPayAsync(@event.InvoiceId, @event.PaymentId);
            }
            catch (System.Exception)
            {
                await RollBackInventoryTransactionAsync(@event.InvoiceId);
            }
        }

        public async Task Handle(InvoiceFailedToPaiedEvent @event)
        {
            //Do some work and rollback ...

            await RollBackInventoryTransactionAsync(@event.InvoiceId);

            await Task.CompletedTask;
        }

        private async Task RollBackInventoryTransactionAsync(Guid invoiceId)
        {
            var invoice = await invoiceService.GetByIdAsync(invoiceId);
            
            var rollBackEvent = new InventoryInvoiceItemsRollBackEvent();

            foreach (var item in invoice.InvoiceItemLines)
            {
                rollBackEvent.ItemLines.Add(new InvoiceItemDto() 
                { 
                    ItemId = item.ItemId,
                    Quantity = item.Quantity
                });
            }
            eventBus.Publish(rollBackEvent);
        }
    }
}
