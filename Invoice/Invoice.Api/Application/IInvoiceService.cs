using EventBus;
using Invoice.Api.Application.Events;
using Invoice.Data;
using Share.IntegrationEvents.Invoice;
using System;
using System.Threading.Tasks;

namespace Invoice.Api.Application
{
    public interface IInvoiceService
    {
        Task<Domain.Invoice> GetByIdAsync(Guid id);
        Task MarkAsPayAsync(Guid invoiceId, Guid paymentId);
        void Add(Invoice.Domain.Invoice invoice);
        Task StartInvoicePaymentAsync(Guid invoiceid);
    }

    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository invoiceRepository;
        private readonly IInvoiceEventService invoiceEventService;
        private readonly IEventBus eventBus;

        public InvoiceService(IInvoiceRepository invoiceRepository, IInvoiceEventService invoiceEventService, IEventBus eventBus)
        {
            this.invoiceRepository = invoiceRepository;
            this.invoiceEventService = invoiceEventService;
            this.eventBus = eventBus;
        }

        public async Task<Domain.Invoice> GetByIdAsync(Guid id)
        {
            return await invoiceRepository.GetAsync(id);
        }

        public void Add(Invoice.Domain.Invoice invoice)
        {
            invoiceRepository.Add(invoice);
        }

        public async Task MarkAsPayAsync(Guid invoiceId, Guid paymentId)
        {
            var invoice = await this.GetByIdAsync(invoiceId);

            invoice.Status = Domain.InvoiceStatus.Paid;
            invoice.PaidAmount = invoice.TotalAmount;

            invoiceRepository.Update(invoice);

            await invoiceRepository.UnitOfWrok.SaveChangesAsync();
        }

        public async Task StartInvoicePaymentAsync(Guid invoiceid)
        {
            var invoice = await this.GetByIdAsync(invoiceid);

            if (invoice.Status == Domain.InvoiceStatus.Paid)
            {
                return;
            }

            var startPayment = new StartInvoicePaymentEvent();
            startPayment.InvoiceId = invoiceid;

            foreach (var item in invoice.InvoiceItemLines)
            {
                startPayment.ItemLines.Add(new InvoiceItemDto(item.ItemId, item.Quantity));
            }
            eventBus.Publish(startPayment);
            //await invoiceEventService.AddAndSaveEventAsync(startPayment);
        }

    }

}
