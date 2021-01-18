using System;
using System.Threading.Tasks;
using Invoice.Api.Application;
using Invoice.Api.Application.Event;
using Invoice.Api.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Payment.Api.Controllers
{
    [ApiController]

    public class InvoiceController : ControllerBase
    {
        private readonly ILogger<InvoiceController> _logger;
        private readonly IInvoiceService invoiceService;

        public InvoiceController(ILogger<InvoiceController> logger, IInvoiceService invoiceService)
        {
            _logger = logger;
            this.invoiceService = invoiceService;
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
