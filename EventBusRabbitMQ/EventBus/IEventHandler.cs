using System.Threading.Tasks;

namespace EventBus.RabbitMQ
{
    public interface IEventHandler<TEvent> where TEvent: Event
    {
        Task Handle(TEvent @event);
    }
}
