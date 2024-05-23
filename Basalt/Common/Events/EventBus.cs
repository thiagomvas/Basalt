using Basalt.Common.Utils;
using Basalt.Core.Common.Abstractions.Engine;
using System;
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
		/// Notifies all observers to render.
		/// </summary>
		public void NotifyRender()
		{
			lock (lockObject)
			{
				if (!eventHandlers.ContainsKey(BasaltConstants.RenderEventKey))
				{
					eventHandlers[BasaltConstants.RenderEventKey] = null;
				}
				eventHandlers[BasaltConstants.RenderEventKey]?.Invoke(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Notifies all observers to start.
		/// </summary>
		public void NotifyStart()
		{
			lock (lockObject)
			{
				if(!eventHandlers.ContainsKey(BasaltConstants.StartEventKey))
				{
					eventHandlers[BasaltConstants.StartEventKey] = null;
				}
				eventHandlers[BasaltConstants.StartEventKey]?.Invoke(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Notifies all observers to update.
		/// </summary>
		public void NotifyUpdate()
		{
			lock (lockObject)
			{
				if(!eventHandlers.ContainsKey(BasaltConstants.UpdateEventKey))
				{
					eventHandlers[BasaltConstants.UpdateEventKey] = null;
				}
				eventHandlers[BasaltConstants.UpdateEventKey]?.Invoke(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Notifies all observers of a physics update.
		/// </summary>
		public void NotifyPhysicsUpdate()
		{
			lock (lockObject)
			{
				if(!eventHandlers.ContainsKey(BasaltConstants.PhysicsUpdateEventKey))
				{
					eventHandlers[BasaltConstants.PhysicsUpdateEventKey] = null;
				}
				eventHandlers[BasaltConstants.PhysicsUpdateEventKey]?.Invoke(this, EventArgs.Empty);
			}
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
	}
}
