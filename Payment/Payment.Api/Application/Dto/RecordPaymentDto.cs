using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Api.Application.Dto
{
    public class RecordPaymentDto
    {
        public decimal Amount { get; set; }
        public Guid InvoiceId { get; set; }
    }
}
