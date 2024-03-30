using Basalt.Core.Common.Abstractions;
namespace Basalt.Common.Events
{
	/// <summary>
	/// Represents an event bus that allows subscribing to and notifying observers of events.
	/// </summary>
	public class EventBus : IEventBus
	{
		private readonly List<IObserver> observers;
		private readonly object lockObject;

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
				foreach (var observer in observers)
				{
					observer.OnRender();
				}
			}
		}

		/// <summary>
		/// Notifies all observers to start.
		/// </summary>
		public void NotifyStart()
		{
			foreach (var observer in observers)
			{
				observer.OnStart();
			}
		}

		/// <summary>
		/// Notifies all observers to update.
		/// </summary>
		public void NotifyUpdate()
		{
			Task.Run(() =>
			{
				lock (lockObject)
				{
					foreach (var observer in observers)
					{
						observer.OnUpdate();
					}
				}
			}).Wait();
		}

		/// <summary>
		/// Notifies all observers of a physics update.
		/// </summary>
		public void NotifyPhysicsUpdate()
		{
			Task.Run(() =>
			{
				lock (lockObject)
				{
					foreach (var observer in observers)
					{
						observer.OnPhysicsUpdate();
					}
				}
			}).Wait();
		}

		/// <summary>
		/// Subscribes an observer to the event bus.
		/// </summary>
		/// <param name="observer">The observer to subscribe.</param>
		public void Subscribe(IObserver observer)
		{
			lock (lockObject)
			{
				observers.Add(observer);
			}
		}

		/// <summary>
		/// Unsubscribes an observer from the event bus.
		/// </summary>
		/// <param name="observer">The observer to unsubscribe.</param>
		public void Unsubscribe(IObserver observer)
		{
			lock (lockObject)
			{
				observers.Remove(observer);
			}
		}
	}
}
