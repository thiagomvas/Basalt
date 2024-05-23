
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
		/// <param name="eventName">The name of the event to subscribe to.</param>
		/// <param name="handler">The handler to call when the event is raised.</param>
		void Subscribe(string eventName, EventHandler handler);

		/// <summary>
		/// Unsubscribes an observer from the event bus.
		/// </summary>
		/// <param name="eventName">The name of the event to unsubscribe from.</param>
		/// <param name="handler">The handler to remove from the event.</param>
		void Unsubscribe(string eventName, EventHandler handler);
		
		/// <summary>
		/// Triggers an event on the event bus.
		/// </summary>
		/// <param name="eventName">The name of the event to trigger.</param>
		void TriggerEvent(string eventName);

	}
}
