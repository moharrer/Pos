using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoice.Api.Application.Event
{
    public class InvoicePaymentSucceedEvent: EventBus.RabbitMQ.Event
    {
        public Guid InvoiceId { get; set; }
        public decimal PaiedAmount { get; set; }
    }
}
