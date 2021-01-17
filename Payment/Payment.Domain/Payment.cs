using Infrastructure;
using System;

namespace Payment.Domain
{
    public class Payment : BaseEntity
    {
        public Guid InvoiceId { get; set; }
        public decimal Amount { get; set; }
    }

}
