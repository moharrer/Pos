using System;
using System.Collections.Generic;
using System.Text;

namespace Share.IntegrationEvents.Invoice
{
    public class InvoiceFailedToPaiedEvent : EventBus.Event
    {
        public InvoiceFailedToPaiedEvent(){}

        public InvoiceFailedToPaiedEvent(Guid invoiceId, decimal amount)
        {
            InvoiceId = invoiceId;
            Amount = amount;
        }

        public Guid InvoiceId { get; set; }
        public decimal Amount { get; set; }
    }
}
