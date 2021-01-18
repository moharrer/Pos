using Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invoice.Domain
{
    public class InvoiceItemLine : BaseEntity
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
    }
}
