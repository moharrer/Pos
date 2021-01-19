using Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invoice.Domain
{
    public class InvoiceItemLine : BaseEntity
    {
        public InvoiceItemLine()
        {

        }
        public InvoiceItemLine(string type, string name,Guid itemId, int quantity)
        {
            this.Id = Guid.NewGuid();
            this.Type = type;
            this.Name = name;
            this.ItemId = itemId;
            this.Quantity = quantity;
        }
        public string Type { get; set; }
        public string Name { get; set; }
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
    }
}
