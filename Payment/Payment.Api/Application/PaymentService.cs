using EventBus;
using Payment.Api.Application.Dto;
using Payment.Api.Application.Event;
using Payment.Api.Application.Events;
using Payment.Data;
using System;
using System.Threading.Tasks;

namespace Payment.Api.Application
{
    public class PaymentService : IPaymentService
    {
        private readonly IEventBus eventBus;
        private readonly IPaymentRepository paymentRepository;
        private readonly IPaymentEventService paymentEventService;

        public PaymentService(IEventBus eventBus, IPaymentRepository paymentRepository, IPaymentEventService paymentEventService)
        {
            this.eventBus = eventBus;
            this.paymentRepository = paymentRepository;
            this.paymentEventService = paymentEventService;
        }
        public Domain.Payment RecordPayment(RecordPaymentDto recordPayment)
        {

            //var @event = new InvoicePaymentSucceedEvent()
            //{
            //    InvoiceId = recordPayment.InvoiceId,
            //    PaiedAmount = recordPayment.Amount
            //};

            var payment = new Domain.Payment
            {
                Id = Guid.NewGuid(),
                InvoiceId = recordPayment.InvoiceId,
                Amount = recordPayment.Amount
            };
            
            paymentRepository.Add(payment);

            return payment;
            //await paymentEventService.AddAndSaveEventAsync(@event);

            //await paymentRepository.UnitOfWrok.SaveChangesAsync();

        }
    }

    public interface IPaymentService
    {
        Domain.Payment RecordPayment(RecordPaymentDto recordPayment);
    }

}
