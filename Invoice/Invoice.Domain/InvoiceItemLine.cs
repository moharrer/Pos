using System;
using System.Collections.Generic;
using System.Text;

namespace Invoice.Domain
{
    public class InvoiceItemLine
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
