using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Core.Common.Abstractions
{
	public interface IObserver
	{
		void OnStart();
		void OnUpdate();
		void OnPhysicsUpdate();
		void OnRender();
	}
}
