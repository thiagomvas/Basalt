using Basalt.Core.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			});
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
			});
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
