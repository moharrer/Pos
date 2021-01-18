using Infrastructure;
using Invoice.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Invoice.Data
{
    public interface IInvoiceRepository : IRepository<Domain.Invoice>
    {
        Domain.Invoice Add(Invoice.Domain.Invoice Invoice);

        void Update(Domain.Invoice Invoice);

        Task<Domain.Invoice> GetAsync(int InvoiceId);
        Task<Domain.Invoice> GetAsync(Guid InvoiceId);
    }

    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly InvoiceContext context;

        public IUnitOfWork UnitOfWrok { get { return context; } }

        public InvoiceRepository(InvoiceContext dbContext)
        {
            this.context = dbContext;
        }

        public Domain.Invoice Add(Domain.Invoice invoice)
        {
            return context.Invoices.Add(invoice).Entity;
        }

        public Task<Domain.Invoice> GetAsync(int InvoiceId)
        {
            var result = context.Invoices.SingleOrDefaultAsync(a => a.Key == InvoiceId);
            return result;
        }

        public Task<Domain.Invoice> GetAsync(Guid InvoiceId)
        {
            var result = context.Invoices.SingleOrDefaultAsync(a => a.Id == InvoiceId);
            return result;
        }

        public void Update(Domain.Invoice Invoice)
        {
            context.Entry(Invoice).State = EntityState.Modified;
        }
    }
}
