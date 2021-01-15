using EventBus.RabbitMQ;
using Payment.Api.Application.Dto;
using Payment.Api.Application.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Api.Application
{
    public class PaymentService : IPaymentService
    {
        private readonly IEventBus eventBus;

        public PaymentService(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }
        public async Task RecordPaymentAsync(RecordPaymentDto recordPayment)
        { 
            var @event = new InvoicePaymentSucceedEvent() 
            { 
                InvoiceId = recordPayment.InvoiceId,
                 PaiedAmount = recordPayment.Amount
            };

            eventBus.Publish(@event);

            await Task.CompletedTask;
    }
    }

    public interface IPaymentService
    {
        Task RecordPaymentAsync(RecordPaymentDto recordPayment);
    }

}
