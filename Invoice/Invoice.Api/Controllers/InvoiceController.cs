using System;
using System.Threading.Tasks;
using EventBus;
using Invoice.Api.Application;
using Invoice.Api.Application.Event;
using Invoice.Api.ViewModel;
using Invoice.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Share.IntegrationEvents.Payment;

namespace Payment.Api.Controllers
{
    [ApiController]

    public class InvoiceController : ControllerBase
    {
        private readonly ILogger<InvoiceController> _logger;
        private readonly IInvoiceService invoiceService;
        private readonly IEventBus eventBus;

        public InvoiceController(ILogger<InvoiceController> logger, IInvoiceService invoiceService, IEventBus eventBus)
        {
            _logger = logger;
            this.invoiceService = invoiceService;
            this.eventBus = eventBus;
        }

        //https://localhost:5004/invoice?id=95895B27-1FB2-4B29-9DF0-5D46BCF364EC
        [Route("[controller]")]
        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out Guid parsedId))
            {
                return BadRequest();
            }

            var result = await invoiceService.GetByIdAsync(parsedId);

            return new JsonResult(result);
        }

        [Route("invoice/Create")]
        [HttpGet]
        public IActionResult Create()
        {
            var invoice = new Invoice.Domain.Invoice() { TotalAmount = 1000, Id = Guid.NewGuid() };

            var itemLine1 = new InvoiceItemLine("Product", "Product 1", new Guid("3BF543C8-8776-4A23-87FD-FCFFB3F7CE55"), 3);
            var itemLine2 = new InvoiceItemLine("Product", "Product 2", new Guid("dBF543C8-8776-4A23-87FD-FCFFB3F7CE69"), 5);
            
            invoice.InvoiceItemLines.Add(itemLine1);
            invoice.InvoiceItemLines.Add(itemLine2);

            invoiceService.Add(invoice);

            return Ok();
        }

        [Route("invoice/recordPayment")]
        [HttpPost]
        public async Task<IActionResult> RecordPayment([FromBody] InvoiceModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            await invoiceService.StartInvoicePaymentAsync(model.InvoiceId);

            return Ok();
        }

    }
}
