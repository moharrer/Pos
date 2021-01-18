using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoice.Api.Application.Event
{
    public class InventoryItemBalancedEvent : EventBus.Event
    {
        public Guid InvoiceId { get; set; }
    }

    public class InventoryItemBalancedFailed : EventBus.Event
    {
        public Guid InvoiceId { get; set; }
        public string Description { get; set; }
    }

}
