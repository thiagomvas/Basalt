using Basalt.Core.Common.Abstractions;
using Basalt.Core.Common.Abstractions.Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Basalt
{
	public class EngineBuilder : IEngineBuilder
	{
		public IGraphicsEngine? GraphicsEngine {get; set; }
		public IPhysicsEngine? PhysicsEngine {get; set; }
		public ISoundSystem? SoundEngine {get; set; }
		public ILogger? Logger { get; set; }
		public IEventBus? EventBus { get; set; }

		public IEngine Build()
		{
			Engine result = new()
			{
				GraphicsEngine = GraphicsEngine,
				PhysicsEngine = PhysicsEngine,
				SoundSystem = SoundEngine,
				Logger = Logger,
				EventBus = EventBus
			};

			return result;
		}

		public IEngineBuilder WithGraphicsEngine(IGraphicsEngine graphicsEngine)
		{
			GraphicsEngine = graphicsEngine;
			return this;
		}

		public IEngineBuilder WithLogger(ILogger logger)
		{
			this.Logger = logger;
			return this;
		}

		public IEngineBuilder WithPhysicsEngine(IPhysicsEngine physicsEngine)
		{
			PhysicsEngine = physicsEngine;
			return this;
		}

		public IEngineBuilder WithSoundEngine(ISoundSystem soundEngine)
		{
			SoundEngine = soundEngine;
			return this;
		}
	}
}
