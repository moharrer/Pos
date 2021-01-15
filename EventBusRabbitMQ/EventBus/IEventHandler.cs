using System.Threading.Tasks;

namespace EventBusRabbitMQ
{
    public interface IEventHandler<TEvent> where TEvent: Event
    {
        Task Handle(TEvent @event);
    }
}
