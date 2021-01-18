using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Api.Application.Event
{
    public class StartInvoicePaymentEvent : EventBus.Event
    {
        public StartInvoicePaymentEvent()
        {
            ItemLines = new List<InvoiceItemDto>();
        }
        public Guid InvoiceId { get; set; }
        public ICollection<InvoiceItemDto> ItemLines { get; set; }
    }

    public class InvoiceItemDto
    {
        public InvoiceItemDto() { }
        public InvoiceItemDto(Guid itemId, int quantity)
        {
            this.ItemId = ItemId;
            this.Quantity = quantity;
        }

        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
    }

}
