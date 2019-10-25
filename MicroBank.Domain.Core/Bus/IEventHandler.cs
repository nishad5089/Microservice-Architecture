using System.Threading.Tasks;

namespace MicroBank.Domain.Core.Bus
{
    public interface IEventHandler<in TEvent> : IEventHandler
    {
         Task Handle(TEvent @event);
    }

    public interface IEventHandler
    {

    }
}