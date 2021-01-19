using EventBus;
using EventLogStore.Ef;
using Inventory.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Inventory.Api.Application.Events
{
    public interface IInventoryEventService
    {
        Task PublishEventsThroughEventBusAsync(Guid transactionId);
        Task AddAndSaveEventAsync(EventBus.Event evt);
    }
    public class InventoryEventService: IInventoryEventService
    {
        private readonly IEventBus eventBus;
        private readonly InventoryContext paymentContext;
        private readonly EventLogDataContext eventLogContext;
        private readonly IEventLogService eventLogService;
        private readonly Func<DbConnection, IEventLogService> eventLogServiceFactory;
        private readonly ILogger<InventoryEventService> logger;

        public InventoryEventService(IEventBus eventBus,
            InventoryContext paymentContext,
            EventLogDataContext eventLogContext,
            Func<DbConnection, IEventLogService> eventLogServiceFactory,
            ILogger<InventoryEventService> logger)
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
