using Basalt.Core.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Core.Common
{
	public class Engine
	{
		private readonly IGraphicsEngine? _graphicsEngine;
		private readonly ISoundSystem? _soundSystem;
		private readonly IPhysicsEngine? _physicsEngine;

		public Engine(IGraphicsEngine? graphicsEngine, ISoundSystem? soundSystem, IPhysicsEngine? physicsEngine)
		{
			_graphicsEngine = graphicsEngine;
			_soundSystem = soundSystem;
			_physicsEngine = physicsEngine;
		}

		public void Initialize()
		{
			_graphicsEngine?.Initialize();
			_soundSystem?.Initialize();
			_physicsEngine?.Initialize();

			Console.WriteLine($"Graphics Engine: {_graphicsEngine?.GetType()}");
			Console.WriteLine($"Sound Engine: {_soundSystem?.GetType()}");
			Console.WriteLine($"Physics Engine: {_physicsEngine?.GetType()}");
		}
	}
}
