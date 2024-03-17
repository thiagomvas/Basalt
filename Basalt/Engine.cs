using Basalt.Common.Entities;
using Basalt.Core.Common.Abstractions;

namespace Basalt
{
	public class Engine : IEngine
	{
		private static Engine? _instance;
		private readonly IGraphicsEngine? _graphicsEngine;
		private readonly ISoundSystem? _soundSystem;
		private readonly IPhysicsEngine? _physicsEngine;
		internal ILogger? logger;
		private readonly IEventBus? _eventBus;
		private bool _exceptionOccurred = false;
		private readonly EntityManager entityManager = new();

		private Thread graphicsThread, physicsThread;
		private Engine(IGraphicsEngine? graphicsEngine, ISoundSystem? soundSystem, IPhysicsEngine? physicsEngine, IEventBus? eventBus = null)
		{
			_graphicsEngine = graphicsEngine;
			_soundSystem = soundSystem;
			_physicsEngine = physicsEngine;
			_eventBus = eventBus;
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

		public static void Initialize(IGraphicsEngine? graphicsEngine, ISoundSystem? soundSystem, IPhysicsEngine? physicsEngine, IEventBus? eventBus = null)
		{
			if (_instance != null)
			{
				throw new InvalidOperationException("Engine has already been initialized.");
			}
			_instance = new Engine(graphicsEngine, soundSystem, physicsEngine, eventBus);
		}

		public void Run()
		{
			logger?.LogInformation("Engine Initializing");
			if (_graphicsEngine == null)
			{
				logger?.LogFatal("Graphics engine not specified! Cannogivet run engine.");
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

			_soundSystem?.Initialize();

			physicsThread = new Thread(() => SafeInitialize(_physicsEngine));
			physicsThread.Start();

			graphicsThread = new Thread(() => SafeInitialize(_graphicsEngine));
			graphicsThread.Start();

			physicsThread.Join();
			graphicsThread.Join();

			if (_exceptionOccurred)
			{
				Shutdown();
				return;
			}
		}

		public void Shutdown()
		{
			logger?.LogWarning("Engine shutting down");
			if (physicsThread != null && physicsThread.IsAlive)
			{
				_physicsEngine?.Shutdown();
			}

			if (graphicsThread != null && graphicsThread.IsAlive)
			{
				_graphicsEngine?.Shutdown();
			}
			logger?.LogInformation("Engine shut down");
		}

		public static void CreateEntity(Entity entity)
		{
			Instance.entityManager.AddEntity(entity);
		}

		public static void RemoveEntity(Entity entity)
		{
			Instance.entityManager.RemoveEntity(entity);
		}

		private void SafeInitialize(IEngineComponent? component)
		{
			try
			{
				component?.Initialize();
			}
			catch (Exception e)
			{
				_exceptionOccurred = true;
				logger?.LogFatal($"EXCEPTION OCURRED AT {component?.GetType().Name}: {e.Message}");
				Shutdown();
				return;
			}
		}

		public IEventBus? EventBus => _eventBus;
	}
}
