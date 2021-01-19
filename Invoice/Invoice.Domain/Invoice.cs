using Infrastructure;
using System;
using System.Collections.Generic;

namespace Invoice.Domain
{
    public class Invoice : BaseEntity
    {
        public Invoice()
        {
            InvoiceItemLines = new List<InvoiceItemLine>();
        }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public InvoiceStatus Status { get; set; }
        public ICollection<InvoiceItemLine> InvoiceItemLines { get; set; }

    }

    public enum InvoiceStatus
    {
        Outstanding = 1,
        Paid = 2
    }
}
