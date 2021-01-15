using EventBus.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoice.Api.Application.Event
{
    public class InvoicePaymentSucceedEventHandler : IEventHandler<InvoicePaymentSucceedEvent>
    {
        private readonly IEventBus eventBus;

        public InvoicePaymentSucceedEventHandler(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        public async Task Handle(InvoicePaymentSucceedEvent @event)
        {


            //eventBus.Publish(orderPaymentIntegrationEvent);

            await Task.CompletedTask;
        }
    }
}
