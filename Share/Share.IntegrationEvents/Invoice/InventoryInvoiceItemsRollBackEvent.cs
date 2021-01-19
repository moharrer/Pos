using System;
using System.Collections.Generic;
using System.Text;

namespace Share.IntegrationEvents.Invoice
{
    public class InventoryInvoiceItemsRollBackEvent : EventBus.Event
    {
        public InventoryInvoiceItemsRollBackEvent()
        {
            ItemLines = new List<InvoiceItemDto>();
        }
        public ICollection<InvoiceItemDto> ItemLines { get; set; }
    }
}
