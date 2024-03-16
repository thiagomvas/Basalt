using Basalt.Core.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Basalt.Core.Common
{
	public class EngineBuilder : IEngineBuilder
	{
		private IGraphicsEngine? graphics;
		private IPhysicsEngine? physics;
		private ISoundSystem? sound;
		private ILogger? logger;

		public Engine Build()
		{
			Engine.Initialize(graphics, sound, physics);

			Engine.Instance.logger = logger;

			return Engine.Instance;
		}

		public IEngineBuilder WithGraphicsEngine(IGraphicsEngine graphicsEngine)
		{
			graphics = graphicsEngine;
			return this;
		}

		public IEngineBuilder WithLogger(ILogger logger)
		{
			this.logger = logger;
			return this;
		}

		public IEngineBuilder WithPhysicsEngine(IPhysicsEngine physicsEngine)
		{
			physics = physicsEngine;
			return this;
		}

		public IEngineBuilder WithSoundEngine(ISoundSystem soundEngine)
		{
			sound = soundEngine;
			return this;
		}
	}
}
