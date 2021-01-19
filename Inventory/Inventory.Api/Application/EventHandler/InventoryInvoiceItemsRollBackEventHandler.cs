using EventBus;
using Inventory.Data;
using Product.Api.Application;
using Share.IntegrationEvents.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Api.Application.EventHandler
{
    public class InventoryInvoiceItemsRollBackEventHandler : IEventHandler<InventoryInvoiceItemsRollBackEvent>
    {
        private readonly IEventBus eventBus;
        private readonly IProductService productService;
        private readonly IProductRepository productRepository;

        public InventoryInvoiceItemsRollBackEventHandler(IEventBus eventBus, IProductService productService, IProductRepository productRepository)
        {
            this.eventBus = eventBus;
            this.productService = productService;
            this.productRepository = productRepository;
        }

        public async Task Handle(InventoryInvoiceItemsRollBackEvent @event)
        {
            var products = await productService.GetAllAsync();

            foreach (var item in @event.ItemLines)
            {
                var product = products.SingleOrDefault(a => a.Id == item.ItemId);

                //Roll back
                if (product != null)
                {
                    product.Quantity += item.Quantity;
                }

                productService.Update(product);

            }
            await productRepository.UnitOfWrok.SaveChangesAsync();

        }
    }
}
