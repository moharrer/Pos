using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Data
{
    public interface IProductRepository : IRepository<Inventory.Domain.Product>
    {
        Task<IEnumerable<Inventory.Domain.Product>> GetAllAsync();
        Inventory.Domain.Product Add(Inventory.Domain.Product Invoice);

        void Update(Inventory.Domain.Product Invoice);

        Task<Inventory.Domain.Product> GetAsync(int InvoiceId);
        Task<Inventory.Domain.Product> GetAsync(Guid InvoiceId);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly InventoryContext context;

        public IUnitOfWork UnitOfWrok { get { return context; } }

        public ProductRepository(InventoryContext dbContext)
        {
            this.context = dbContext;
        }

        public async Task<IEnumerable<Domain.Product>> GetAllAsync()
        {
            return await context.Products.ToListAsync();
        }

        public Domain.Product Add(Inventory.Domain.Product invoice)
        {
            return context.Products.Add(invoice).Entity;
        }

        public Task<Domain.Product> GetAsync(int InvoiceId)
        {
            var result = context.Products.SingleOrDefaultAsync(a => a.Key == InvoiceId);
            return result;
        }

        public Task<Domain.Product> GetAsync(Guid InvoiceId)
        {
            var result = context.Products.SingleOrDefaultAsync(a => a.Id == InvoiceId);
            return result;
        }

        public void Update(Domain.Product Invoice)
        {
            context.Entry(Invoice).State = EntityState.Modified;
        }
    }
}
