using System;
using System.Collections.Generic;
using System.Text;

namespace Share.IntegrationEvents.Payment
{
    public class RequestPaymentEvent : EventBus.Event
    {
        public Guid InvoiceId { get; set; }
        public decimal Amount { get; set; }
    }
}
