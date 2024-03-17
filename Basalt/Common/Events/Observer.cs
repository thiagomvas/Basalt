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
			Raylib.DrawSphere(pos, 2, Color.Red);
		}

		public void OnStart()
		{
			pos = new(0, 0, 0);
		}

		public void OnUpdate()
		{
			pos = new(0, (float)Math.Sin(Raylib.GetTime() / 10d) * 10, 0); 
		}
	}
}
