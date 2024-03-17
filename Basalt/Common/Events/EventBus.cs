using Basalt.Core.Common.Abstractions;
namespace Basalt.Common.Events
{
	public class EventBus : IEventBus
	{
		private readonly List<IObserver> observers;
		private readonly object lockObject;

		public EventBus()
		{
			observers = new List<IObserver>();
			lockObject = new object();
		}

		public void NotifyRender()
		{
			Task.Run(() =>
			{
				lock (lockObject)
				{
					foreach (var observer in observers)
					{
						observer.OnRender();
					}
				}
			}).Wait();
		}

		public void NotifyStart()
		{
			lock (lockObject)
			{
				foreach (var observer in observers)
				{
					observer.OnStart();
				}
			}
		}

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

		public void Subscribe(IObserver observer)
		{
			lock (lockObject)
			{
				observers.Add(observer);
			}
		}

		public void Unsubscribe(IObserver observer)
		{
			lock (lockObject)
			{
				observers.Remove(observer);
			}
		}
	}
}
