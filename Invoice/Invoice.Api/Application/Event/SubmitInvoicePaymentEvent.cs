using System;

namespace Invoice.Api.Application.Event
{
    public class RecordInvoicePaymentEvent : EventBus.Event
    {
        public Guid InvoiceId { get; set; }
        public decimal PaidAmount { get; set; }
    }
}
