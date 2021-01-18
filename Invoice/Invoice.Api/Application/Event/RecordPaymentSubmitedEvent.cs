using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoice.Api.Application.Event
{
    public class RecordPaymentSubmitedEvent : EventBus.Event
    {
        public Guid PaymentId { get; set; }
        public Guid InvoiceId { get; set; }
        public decimal Amount { get; set; }
    }
}
