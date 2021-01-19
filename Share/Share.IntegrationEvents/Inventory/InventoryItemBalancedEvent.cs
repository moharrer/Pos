using System;

namespace Share.IntegrationEvents.Inventory
{
    public class InventoryItemBalancedEvent : EventBus.Event
    {
        public Guid InvoiceId { get; set; }
    }
}
