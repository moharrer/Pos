using EventBus;
using EventBus.RabbitMQ;
using Invoice.Api.Application.Events;
using Invoice.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoice.Api.Application.Event
{
    public class InvoiceEventHandler : IEventHandler<RecordPaymentSubmitedEvent>
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

        public async Task Handle(RecordPaymentSubmitedEvent @event)
        {
            await invoiceService.MarkAsPayAsync(@event.InvoiceId, @event.PaymentId);
        }
        
    }
}
