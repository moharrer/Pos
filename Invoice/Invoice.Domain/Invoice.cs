using Infrastructure;
using System;
using System.Collections.Generic;

namespace Invoice.Domain
{
    public class Invoice : BaseEntity
    {
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public InvoiceStatus Status { get; set; }
        public Guid PaymentId { get; set; }
        public ICollection<InvoiceItemLine> InvoiceItemLines { get; set; }

    }

    public enum InvoiceStatus
    {
        Outstanding = 1,
        Paid = 2
    }
}
