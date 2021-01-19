using EventBus;
using Payment.Api.Application.Events;
using Payment.Data;
using Share.IntegrationEvents;
using Share.IntegrationEvents.Invoice;
using Share.IntegrationEvents.Payment;
using System.Threading.Tasks;

namespace Payment.Api.Application.Event
{
    public class PaymentEventHandler : IEventHandler<RequestPaymentEvent>
    {
        private readonly IEventBus eventBus;
        private readonly IPaymentService paymentService;
        private readonly IPaymentRepository paymentRepository;

        public PaymentEventHandler(IEventBus eventBus, IPaymentService paymentService, IPaymentRepository paymentRepository)
        {
            this.eventBus = eventBus;
            this.paymentService = paymentService;
            this.paymentRepository = paymentRepository;
        }

        public async Task Handle(RequestPaymentEvent @event)
        {
            try
            {
                //TODO: do some validation on @event 

                var payment = paymentService.RecordPayment(new Dto.RecordPaymentDto()
                {
                    Amount = @event.Amount,
                    InvoiceId = @event.InvoiceId
                });
                
                await paymentRepository.UnitOfWrok.SaveChangesAsync();

                eventBus.Publish(new RecordPaymentSubmitedEvent(payment.Id, @event.InvoiceId, @event.Amount));
            }
            catch (System.Exception)
            {
                //Roll back
                eventBus.Publish(new InvoiceFailedToPaiedEvent(@event.InvoiceId, @event.Amount));
            }

            await Task.CompletedTask;
        }

    }
}
