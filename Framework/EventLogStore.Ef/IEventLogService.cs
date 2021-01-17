using EventBus;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventLogStore.Ef
{
    public interface IEventLogService
    {
        Task<IEnumerable<EventLog>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId);
        Task SaveEventAsync(Event @event, IDbContextTransaction transaction);
        Task MarkEventAsPublishedAsync(Guid eventId);
        Task MarkEventAsFailedAsync(Guid eventId);
    }

}

