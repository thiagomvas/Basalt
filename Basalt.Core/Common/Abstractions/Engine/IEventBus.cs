
namespace Basalt.Core.Common.Abstractions.Engine
{
	/// <summary>
	/// Represents an event bus in the engine.
	/// </summary>
	public interface IEventBus : IEngineComponent
	{
		/// <summary>
		/// Subscribes an observer to the event bus.
		/// </summary>
		/// <param name="observer">The observer to subscribe.</param>
		void Subscribe(IObserver observer);

		/// <summary>
		/// Unsubscribes an observer from the event bus.
		/// </summary>
		/// <param name="observer">The observer to unsubscribe.</param>
		void Unsubscribe(IObserver observer);

		/// <summary>
		/// Notifies the event bus that the engine has started.
		/// </summary>
		void NotifyStart();

		/// <summary>
		/// Notifies the event bus of an engine update.
		/// </summary>
		void NotifyUpdate();

		/// <summary>
		/// Notifies the event bus of a physics update.
		/// </summary>
		void NotifyPhysicsUpdate();

		/// <summary>
		/// Notifies the event bus to render the scene.
		/// </summary>
		void NotifyRender();

		/// <summary>
		/// Checks if an observer is subscribed to the event bus.
		/// </summary>
		/// <param name="observer">The observer to check.</param>
		/// <returns>True if the observer is subscribed, false otherwise.</returns>
		bool IsSubscribed(IObserver observer);
	}
}
