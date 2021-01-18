using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoice.Api.Application.Dto
{
    public class InvoiceDto
    {
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
    }
}
