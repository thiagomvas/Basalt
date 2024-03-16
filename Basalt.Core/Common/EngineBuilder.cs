using Basalt.Core.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Core.Common
{
	public class EngineBuilder : IEngineBuilder
	{
		private IGraphicsEngine? graphics;
		private IPhysicsEngine? physics;
		private ISoundSystem? sound;

		public Engine Build()
		{
			return new Engine(graphics, sound, physics);
		}

		public IEngineBuilder UseGraphicsEngine(IGraphicsEngine graphicsEngine)
		{
			graphics = graphicsEngine;
			return this;
		}

		public IEngineBuilder UsePhysicsEngine(IPhysicsEngine physicsEngine)
		{
			physics = physicsEngine;
			return this;
		}

		public IEngineBuilder UseSoundEngine(ISoundSystem soundEngine)
		{
			sound = soundEngine;
			return this;
		}
	}
}
