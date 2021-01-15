using System.Collections.Generic;
using System.Text;

namespace EventBusRabbitMQ
{
    public interface IEventBus
    {
        public interface IEventBus
        {
            void Publish(Event @event);

            void Subscribe<T, TH>()
                where T : Event
                where TH : IEventHandler<T>;
        }
    }
}
