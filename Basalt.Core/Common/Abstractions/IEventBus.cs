using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Core.Common.Abstractions
{
	public interface IEventBus
	{
		void Subscribe(IObserver observer);
		void Unsubscribe(IObserver observer);
		void NotifyStart();
		void NotifyUpdate();
		void NotifyPhysicsUpdate();
		void NotifyRender();
	}
}
