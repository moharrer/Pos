using System.Threading.Tasks;

namespace EventBus
{
    public interface IEventHandler<TEvent> where TEvent: Event
    {
        Task Handle(TEvent @event);
    }
}
