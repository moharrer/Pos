using System;

namespace Share.IntegrationEvents
{
    public class RecordPaymentSubmitedEvent : EventBus.Event
    {
        public RecordPaymentSubmitedEvent(){}
        public RecordPaymentSubmitedEvent(Guid paymentId, Guid invoiceId, decimal amount)
        {
            PaymentId = paymentId;
            InvoiceId = invoiceId;
            Amount = amount;
        }
        public Guid PaymentId { get; set; }
        public Guid InvoiceId { get; set; }
        public decimal Amount { get; set; }
    }
}
