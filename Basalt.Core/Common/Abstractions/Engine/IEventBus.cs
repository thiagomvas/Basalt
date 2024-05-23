
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
		void Subscribe(string eventName, EventHandler handler);

		/// <summary>
		/// Unsubscribes an observer from the event bus.
		/// </summary>
		/// <param name="observer">The observer to unsubscribe.</param>
		void Unsubscribe(string eventName, EventHandler handler);

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

	}
}
