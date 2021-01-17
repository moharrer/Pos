using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Api.Application.Event
{
    public class InvoicePaymentSucceedEvent : EventBus.Event
    {
        public Guid InvoiceId { get; set; }
        public decimal PaiedAmount { get; set; }
    }
}
