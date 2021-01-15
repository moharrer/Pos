using System;

namespace Invoice.Domain
{
    public class Invoice
    {

        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
    }

    public enum InvoiceStatus
    {
        Outstanding = 1,
        Paid = 2
    }
}
