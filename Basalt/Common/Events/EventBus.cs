using Basalt.Core.Common.Abstractions.Engine;
namespace Basalt.Common.Events
{
	/// <summary>
	/// Represents an event bus that allows subscribing to and notifying observers of events.
	/// </summary>
	public class EventBus : IEventBus
	{
		private readonly List<IObserver> observers;
		private readonly object lockObject;

		// Game Events
		private Dictionary<string, EventHandler> eventHandlers = new Dictionary<string, EventHandler>();
		/// <summary>
		/// Initializes a new instance of the <see cref="EventBus"/> class.
		/// </summary>
		public EventBus()
		{
			observers = new List<IObserver>();
			lockObject = new object();
		}

		/// <summary>
		/// Subscribes an observer to the event bus.
		/// </summary>
		/// <param name="observer">The observer to subscribe.</param>
		public void Subscribe(string eventName, EventHandler handler)
		{
			lock (lockObject)
			{
				if (!eventHandlers.ContainsKey(eventName))
				{
					eventHandlers[eventName] = null;
				}
				eventHandlers[eventName] += handler;
			}
		}

		/// <summary>
		/// Unsubscribes an observer from the event bus.
		/// </summary>
		/// <param name="observer">The observer to unsubscribe.</param>
		public void Unsubscribe(string eventName, EventHandler handler)
		{
			lock (lockObject)
			{
				if (eventHandlers.ContainsKey(eventName))
				{
					eventHandlers[eventName] -= handler;
				}
			}
		}


		public void Initialize()
		{

		}

		public void Shutdown()
		{

		}

		public void TriggerEvent(string eventName)
		{
			lock (lockObject)
			{
				if (eventHandlers.ContainsKey(eventName))
				{
					eventHandlers[eventName]?.Invoke(this, EventArgs.Empty);
				}
			}
		}
	}
}
