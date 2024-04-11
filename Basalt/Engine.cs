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
		public bool Running { get; private set; } = false;

		#region Components & Singleton

		private static Engine? _instance;
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

		private readonly IGraphicsEngine? _graphicsEngine;
		private readonly ISoundSystem? _soundSystem;
		private readonly IPhysicsEngine? _physicsEngine;
		private readonly ILogger? _logger;
		private readonly IEventBus? _eventBus;

        public IGraphicsEngine? GraphicsEngine { get => _graphicsEngine; init => _graphicsEngine = value; }
		public ISoundSystem? SoundSystem { get => _soundSystem; init => _soundSystem = value; }
		public IPhysicsEngine? PhysicsEngine { get => _physicsEngine; init => _physicsEngine = value; }
		public ILogger? Logger { get => _logger; init => _logger = value; }
		public IEventBus? EventBus { get => _eventBus; init => _eventBus = value; }

        private bool _exceptionOccurred = false;
		public readonly EntityManager EntityManager = new();

		#endregion

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
			_soundSystem = soundSystem;
			_physicsEngine = physicsEngine;
			_eventBus = eventBus;
		}

        public Engine()
        {
			_instance = this;
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
			_logger?.LogInformation("Engine Initializing");
			if (_graphicsEngine == null)
			{
				throw new Exception("Graphics engine not specified! Cannot run engine.");
			}
			if (_soundSystem == null)
			{
				_logger?.LogWarning("Sound system not specified! Engine will run without sound.");
			}

			if (_physicsEngine == null)
			{
				_logger?.LogWarning("Physics engine not specified! Engine will run without physics.");
			}

			_logger?.LogInformation("Engine starting");

			graphicsThread = new Thread(() => SafeInitialize(_graphicsEngine));
			graphicsThread.Start();

			physicsThread = new Thread(() => SafeInitialize(_physicsEngine));
			physicsThread.Start();

			Running = true;

			_soundSystem?.Initialize();

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
			// Shut down services in reverse order of initialization
			Running = false;
			_logger?.LogWarning("Engine shutting down");

			
			Task.Run(() => _soundSystem?.Shutdown());
			if (physicsThread != null && physicsThread.IsAlive)
			{
				_physicsEngine?.Shutdown();
			}

			if (graphicsThread != null && graphicsThread.IsAlive)
			{
				_graphicsEngine?.Shutdown();
			}


			_logger?.LogInformation("Engine shut down");
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
#if DEBUG
				_logger?.LogFatal($"EXCEPTION OCCURRED AT {component?.GetType().Name}: {e.GetType().Name} - {e.Message}\n {e.StackTrace}");
#else
				logger?.LogFatal($"EXCEPTION OCCURRED AT {component?.GetType().Name}: {e.GetType().Name} - {e.Message}");
#endif
				_logger?.SaveLog($"CRASH_REPORT_{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.txt");
				_logger?.LogInformation("Saved crash report");
				Shutdown();
			}
		}
	}
}
