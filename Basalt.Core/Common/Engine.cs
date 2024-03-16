using Basalt.Core.Common.Abstractions;

namespace Basalt.Core.Common
{
	public class Engine
	{
		private static Engine? _instance;
		private readonly IGraphicsEngine? _graphicsEngine;
		private readonly ISoundSystem? _soundSystem;
		private readonly IPhysicsEngine? _physicsEngine;
		internal ILogger? logger;
		private Engine(IGraphicsEngine? graphicsEngine, ISoundSystem? soundSystem, IPhysicsEngine? physicsEngine)
		{
			_graphicsEngine = graphicsEngine;
			_soundSystem = soundSystem;
			_physicsEngine = physicsEngine;
		}

		public static Engine Instance
		{
			get
			{
				if (_instance == null)
				{
					throw new InvalidOperationException("Engine has not been initialized.");
				}
				return _instance;
			}
		}

		public static void Initialize(IGraphicsEngine? graphicsEngine, ISoundSystem? soundSystem, IPhysicsEngine? physicsEngine)
		{
			if (_instance != null)
			{
				throw new InvalidOperationException("Engine has already been initialized.");
			}
			_instance = new Engine(graphicsEngine, soundSystem, physicsEngine);
		}

		public void Run()
		{
			logger?.LogInformation("Engine Initializing");
			if (_graphicsEngine == null)
			{
				logger?.LogFatal("Graphics engine not specified! Cannot run engine.");
				return;
			}
			if (_soundSystem == null)
			{
				logger?.LogWarning("Sound system not specified! Engine will run without sound.");
			}

			if (_physicsEngine == null)
			{
				logger?.LogWarning("Physics engine not specified! Engine will run without physics.");
			}

			_graphicsEngine?.Initialize();
			_soundSystem?.Initialize();
			_physicsEngine?.Initialize();
		}
	}
}
