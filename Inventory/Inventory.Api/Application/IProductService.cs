
using Inventory.Api.Application.Events;
using Inventory.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.Api.Application
{
    public interface IProductService
    {
        Task<IEnumerable<Inventory.Domain.Product>> GetAllAsync();
        Task<Inventory.Domain.Product> GetByIdAsync(Guid id);
        void Update(Inventory.Domain.Product product);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository ProductRepository;
        private readonly IInventoryEventService inventoryEventService;

        public ProductService(IProductRepository ProductRepository, IInventoryEventService inventoryEventService)
        {
            this.ProductRepository = ProductRepository;
            this.inventoryEventService = inventoryEventService;
        }

        public async Task<IEnumerable<Inventory.Domain.Product>> GetAllAsync()
        {
            return await ProductRepository.GetAllAsync();
        }

        public async Task<Inventory.Domain.Product> GetByIdAsync(Guid id)
        {
            return await ProductRepository.GetAsync(id);
        }

        public void Update(Inventory.Domain.Product product)
        {
            ProductRepository.Update(product);
        }

    }

}
