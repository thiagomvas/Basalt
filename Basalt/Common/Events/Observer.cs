using Basalt.Core.Common.Abstractions;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Common.Events
{
	public class Observer : IObserver
	{
		private readonly ILogger logger;
		public int Id;
		Vector3 pos;
		public Observer(ILogger logger)
		{
			this.logger = logger;
		}
		public void OnRender()
		{

		}

		public void OnStart()
		{

		}

		public void OnUpdate()
		{

		}
	}
}
