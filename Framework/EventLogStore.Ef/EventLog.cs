using EventBus;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EventLogStore.Ef
{
    public class EventLog
    {
        public EventLog() { }
        public EventLog(Event @event, Guid transactionId)
        {
            EventId = @event.Id;
            CreationTime = @event.CreationDate;
            this.TransactionId = transactionId.ToString();
            this.EventTypeName = @event.GetType().FullName;
            this.Content = JsonConvert.SerializeObject(@event);
        }

        //Event Id
        public Guid EventId { get; set; }

        public DateTime CreationTime { get; private set; }
        public string EventTypeName { get; set; }
        [NotMapped]
        public string EventTypeShortName => EventTypeName.Split('.')?.Last();
        public string Content { get; private set; }
        public string TransactionId { get; private set; }
        public EventStateEnum State { get; set; }
        [NotMapped]
        public Event Event { get; set; }
        public EventLog DeserializeJsonContent(Type type)
        {
            Event = JsonConvert.DeserializeObject(Content, type) as Event;

            return this;
        }

    }
}
