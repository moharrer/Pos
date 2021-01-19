using System;
using System.Collections.Generic;
using System.Text;

namespace Share.IntegrationEvents.Inventory
{
    public class InventoryItemBalancedFailed : EventBus.Event
    {
        public Guid InvoiceId { get; set; }
        public string Description { get; set; }
    }
}
