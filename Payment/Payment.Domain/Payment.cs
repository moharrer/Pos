using System;

namespace Payment.Domain
{
    public class Payment
    {
        public int Key { get; set; }
        public Guid Id { get; set; }
        public Guid InvoiceId { get; set; }
        public decimal Amount { get; set; }
    }

}
