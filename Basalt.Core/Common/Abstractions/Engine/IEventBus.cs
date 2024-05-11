
namespace Basalt.Core.Common.Abstractions.Engine
{
    public interface IEventBus : IEngineComponent
    {
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
        void NotifyStart();
        void NotifyUpdate();
        void NotifyPhysicsUpdate();
        void NotifyRender();
        bool IsSubscribed(IObserver observer);
    }
}
