using EventBus;
using Payment.Api.Application.Events;
using Payment.Data;
using System.Threading.Tasks;

namespace Payment.Api.Application.Event
{
    public class paymentPaymentSucceedEventHandler : IEventHandler<InvoiceFailedToPaiedEvent>
    {
        private readonly IEventBus eventBus;
        private readonly IPaymentService paymentService;
        private readonly PaymentContext paymentContext;
        private readonly IPaymentEventService paymentEventService;
        private readonly IPaymentRepository paymentRepository;

        public paymentPaymentSucceedEventHandler(IEventBus eventBus, IPaymentService paymentService, PaymentContext paymentContext, IPaymentEventService paymentEventService, IPaymentRepository paymentRepository)
        {
            this.eventBus = eventBus;
            this.paymentService = paymentService;
            this.paymentContext = paymentContext;
            this.paymentEventService = paymentEventService;
            this.paymentRepository = paymentRepository;
        }

        public async Task Handle(InvoiceFailedToPaiedEvent @event)
        {
            var payment = paymentService.RecordPayment(new Dto.RecordPaymentDto() 
            { 
                Amount = @event.PaiedAmount, 
                InvoiceId = @event.InvoiceId 
            });
            
            var recordPaymentEvent = new RecordPaymentSubmitedEvent() 
            { 
                InvoiceId = payment.InvoiceId,
                PaymentId = payment.Id,
                Amount = @event.PaiedAmount
            };

            await paymentEventService.AddAndSaveEventAsync(recordPaymentEvent);
        }
    }
}
