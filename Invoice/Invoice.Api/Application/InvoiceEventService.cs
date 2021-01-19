using EventBus;
using EventLogStore.Ef;
using Invoice.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Invoice.Api.Application.Events
{
    public interface IInvoiceEventService
    {
        Task PublishEventsThroughEventBusAsync(Guid transactionId);
        Task AddAndSaveEventAsync(EventBus.Event evt);
    }
    public class InvoiceEventService: IInvoiceEventService
    {
        private readonly IEventBus eventBus;
        private readonly InvoiceContext paymentContext;
        private readonly EventLogDataContext eventLogContext;
        private readonly IEventLogService eventLogService;
        private readonly Func<DbConnection, IEventLogService> eventLogServiceFactory;
        private readonly ILogger<InvoiceEventService> logger;

        public InvoiceEventService(IEventBus eventBus,
            InvoiceContext paymentContext,
            EventLogDataContext eventLogContext,
            Func<DbConnection, IEventLogService> eventLogServiceFactory,
            ILogger<InvoiceEventService> logger)
        {
            this.eventBus = eventBus;
            this.paymentContext = paymentContext;
            this.eventLogContext = eventLogContext;
            this.eventLogService = eventLogServiceFactory(paymentContext.Database.GetDbConnection());
            this.logger = logger;
        }

        public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
        {
            var pendingLogEvents = await eventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId);

            foreach (var logEvt in pendingLogEvents)
            {
                try
                {
                    //await eventLogService.MarkEventAsInProgressAsync(logEvt.EventId);
                    eventBus.Publish(logEvt.Event);
                    await eventLogService.MarkEventAsPublishedAsync(logEvt.EventId);
                }
                catch (Exception ex)
                {
                    await eventLogService.MarkEventAsFailedAsync(logEvt.EventId);
                }
            }
        }

        public async Task AddAndSaveEventAsync(EventBus.Event evt)
        {
            logger.LogInformation("----- Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", evt.Id, evt);

            await eventLogService.SaveEventAsync(evt, paymentContext.GetCurrentTransaction());
        }
    }
}
