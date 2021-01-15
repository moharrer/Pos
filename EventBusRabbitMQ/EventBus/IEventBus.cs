using System.Collections.Generic;
using System.Text;

namespace EventBus.RabbitMQ
{

    public interface IEventBus
    {
        void Publish(Event @event);

        void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>;
    }
}
