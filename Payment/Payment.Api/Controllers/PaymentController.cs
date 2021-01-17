using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Payment.Api.Application;

namespace Payment.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentService paymentService;

        public PaymentController(ILogger<PaymentController> logger, IPaymentService paymentService)
        {
            _logger = logger;
            this.paymentService = paymentService;
        }

        [HttpGet]
        public async Task<IActionResult> RecordPayment(/*[FromBody]InvoicePaymentModel model*/)
        {
           await paymentService.RecordPaymentAsync(new Application.Dto.RecordPaymentDto 
            { 
                InvoiceId = Guid.NewGuid(),//model.InvoiceId, 
                Amount = 100 
            });

            return Ok();
        }
    }
}
