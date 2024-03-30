using Basalt.Common.Entities;
using Basalt.Core.Common.Abstractions;
using Basalt.Core.Common.Abstractions.Sound;

namespace Basalt
{
	/// <summary>
	/// Represents the main game engine. Responsible for initializing any engine components and overall management.
	/// </summary>
	public class Engine : IEngine
	{
		/// <summary>
		/// Gets a value indicating whether the engine has started.
		/// </summary>
		public bool HasStarted { get; private set; } = false;

		private static Engine? _instance;
		private readonly IGraphicsEngine? _graphicsEngine;
		public readonly ISoundSystem? SoundSystem;
		private readonly IPhysicsEngine? _physicsEngine;
		internal ILogger? logger;
		private readonly IEventBus? _eventBus;
		private bool _exceptionOccurred = false;
		public readonly EntityManager EntityManager = new();

		public Action<Entity> OnCreateEntity;

		private Thread graphicsThread, physicsThread;

		/// <summary>
		/// Initializes a new instance of the <see cref="Engine"/> class.
		/// </summary>
		/// <param name="graphicsEngine">The graphics engine.</param>
		/// <param name="soundSystem">The sound system.</param>
		/// <param name="physicsEngine">The physics engine.</param>
		/// <param name="eventBus">The event bus.</param>
		private Engine(IGraphicsEngine? graphicsEngine, ISoundSystem? soundSystem, IPhysicsEngine? physicsEngine, IEventBus? eventBus = null)
		{
			_graphicsEngine = graphicsEngine;
			SoundSystem = soundSystem;
			_physicsEngine = physicsEngine;
			_eventBus = eventBus;
		}

		/// <summary>
		/// Gets the instance of the engine.
		/// </summary>
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

		/// <summary>
		/// Initializes the engine with the specified graphics engine, sound system, physics engine, and event bus.
		/// </summary>
		/// <param name="graphicsEngine">The graphics engine.</param>
		/// <param name="soundSystem">The sound system.</param>
		/// <param name="physicsEngine">The physics engine.</param>
		/// <param name="eventBus">The event bus.</param>
		public static void Initialize(IGraphicsEngine? graphicsEngine, ISoundSystem? soundSystem, IPhysicsEngine? physicsEngine, IEventBus? eventBus = null)
		{
			if (_instance != null)
			{
				throw new InvalidOperationException("Engine has already been initialized.");
			}
			_instance = new Engine(graphicsEngine, soundSystem, physicsEngine, eventBus);
		}

		/// <summary>
		/// Runs the engine.
		/// </summary>
		public void Run()
		{
			logger?.LogInformation("Engine Initializing");
			if (_graphicsEngine == null)
			{
				logger?.LogFatal("Graphics engine not specified! Cannot run engine.");
				return;
			}
			if (SoundSystem == null)
			{
				logger?.LogWarning("Sound system not specified! Engine will run without sound.");
			}

			if (_physicsEngine == null)
			{
				logger?.LogWarning("Physics engine not specified! Engine will run without physics.");
			}

			graphicsThread = new Thread(() => SafeInitialize(_graphicsEngine));
			graphicsThread.Start();

			physicsThread = new Thread(() => SafeInitialize(_physicsEngine));
			physicsThread.Start();

			HasStarted = true;

			SoundSystem?.Initialize();

			_eventBus?.NotifyStart();

			physicsThread.Join();
			graphicsThread.Join();

			if (_exceptionOccurred)
			{
				Shutdown();
				return;
			}
		}

		/// <summary>
		/// Shuts down the engine.
		/// </summary>
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

			SoundSystem?.Shutdown();
			logger?.LogInformation("Engine shut down");
		}

		/// <summary>
		/// Creates an entity and adds it to the entity manager.
		/// </summary>
		/// <param name="entity">The entity to create.</param>
		public static void CreateEntity(Entity entity)
		{
			Instance.EntityManager.AddEntity(entity);
			Instance.OnCreateEntity?.Invoke(entity);
		}

		/// <summary>
		/// Removes an entity from the entity manager.
		/// </summary>
		/// <param name="entity">The entity to remove.</param>
		public static void RemoveEntity(Entity entity)
		{
			Instance.EntityManager.RemoveEntity(entity);
		}

		/// <summary>
		/// Initializes the specified engine component safely.
		/// </summary>
		/// <param name="component">The engine component to initialize.</param>
		private void SafeInitialize(IEngineComponent? component)
		{
			try
			{
				component?.Initialize();
			}
			catch (Exception e)
			{
				_exceptionOccurred = true;
				logger?.LogFatal($"EXCEPTION OCCURRED AT {component?.GetType().Name}: {e.Message}");
				Shutdown();
				return;
			}
		}

		/// <summary>
		/// Gets the event bus.
		/// </summary>
		public IEventBus? EventBus => _eventBus;

		/// <summary>
		/// Gets the logger.
		/// </summary>
		public static ILogger? Logger => _instance?.logger;

		/// <summary>
		/// Gets the physics engine.
		/// </summary>
		public static IPhysicsEngine? PhysicsEngine => _instance?._physicsEngine;

		/// <summary>
		/// Gets the graphics engine.
		/// </summary>
		public static IGraphicsEngine? GraphicsEngine => _instance?._graphicsEngine;
	}
}
