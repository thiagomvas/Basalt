using Basalt.Common.Entities;
using Basalt.Core.Common.Abstractions.Engine;

namespace Basalt
{
	/// <summary>
	/// Represents the core engine of the game. This class is a singleton and cannot be created using a constructor. To set up the engine, use <see cref="Common.EngineBuilder"/>
	/// </summary>
	public class Engine
	{
		#region Singleton
		private static Engine? _instance;

		/// <summary>
		/// The single instance of the engine class.
		/// </summary>
		public static Engine Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new Engine();
				}
				return _instance;
			}
		}

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		internal Engine()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		{
			_instance = this;
		}

		#endregion

		private bool running = false;

		/// <summary>
		/// Indicates whether the engine has started succesfully and is running.
		/// </summary>
		public bool Running
		{
			get => running;
			private set => running = value;
		}

		private Dictionary<Type, ComponentHolder> Components { get; set; } = new();

		/// <summary>
		/// The entity manager that holds all the entities.
		/// </summary>
		public EntityManager EntityManager { get; private set; }
		private ILogger? _logger;

		/// <summary>
		/// The logger associated with the engine.
		/// </summary>
		public ILogger? Logger
		{
			get { return _logger; }
			private set { _logger = value; }
		}


		internal void AddComponent(Type type, IEngineComponent component, bool separateThread = false)
		{
			Components.Add(type, new(component, separateThread));
		}

		internal void AddLogger(ILogger logger) => Logger = logger;

		/// <summary>
		/// Tries to get the reference to a component of the engine.
		/// </summary>
		/// <typeparam name="T">The interface responsible for holding the actual type, such as <see cref="IGraphicsEngine"/>, <see cref="IEventBus"/>. </typeparam>
		/// <returns>The reference to the implementation of the specified interface if it is attached to the engine using the builder, returns <c>null</c> otherwise</returns>
		public T? GetEngineComponent<T>() where T : IEngineComponent
		{
			if (Components.ContainsKey(typeof(T)))
			{
				return (T)Components[typeof(T)].component;
			}
			else
			{
				Logger?.LogError($"Could not find component of type {typeof(T).Name}.");
				return default;
			}
		}

		/// <summary>
		/// Initializes the engine.
		/// </summary>
		public void Initialize()
		{
			// Block initialization if no graphics engine or event bus is found
			if (!Components.ContainsKey(typeof(IGraphicsEngine)))
			{
				Logger?.LogFatal("Could not find a Graphics Engine component that implements IGraphicsEngine. Cannot run without one.");
				return;
			}

			if (!Components.ContainsKey(typeof(IEventBus)))
			{
				Logger?.LogFatal("Could not find an Event Bus component that implements IEventBus. Cannot run without one.");
				return;
			}

			// Initialize Entity Manager
			EntityManager = new(GetEngineComponent<IEventBus>()!);

			// Move graphics engine and event bus to the front of the list
			Components = Components.OrderBy(c => c.Key == typeof(IGraphicsEngine) ? 0 : c.Key == typeof(IEventBus) ? 1 : 2).ToDictionary(c => c.Key, c => c.Value);

			// Initialize other components
			foreach (var component in Components)
			{
				if (component.Value.separateThread)
				{
					Logger?.LogDebug($"Initializing {component.Value.component.GetType().Name} ({component.Key.Name}) on a separate thread.");
					Thread thread = new(() => component.Value.component.Initialize());
					thread.Start();
				}
				else
				{
					Logger?.LogDebug($"Initializing {component.Value.component.GetType().Name} ({component.Key.Name}) on the main thread.");
					component.Value.component.Initialize();
				}
			}

			Logger?.LogInformation("Engine finished initializing components.");

			Instance.GetEngineComponent<IEventBus>()?.NotifyStart();

			Running = true;
		}

		/// <summary>
		/// Shuts down the engine.
		/// </summary>
		public void Shutdown()
		{
			Running = false;
			Logger?.LogWarning("Shutting down engine...");
			foreach (var component in Components)
			{
				component.Value.component.Shutdown();
			}
		}

		/// <summary>
		/// Creates an entity in the engine.
		/// </summary>
		/// <param name="entity">The entity to create.</param>
		public static void CreateEntity(Entity entity)
		{
			if (!Instance.Running)
			{
				throw new InvalidOperationException("Cannot create entity when engine is not running.");
			}
			Instance.Logger?.LogDebug($"Creating entity {entity.Id}...");
			Instance.EntityManager.AddEntity(entity);
			Instance.GetEngineComponent<IPhysicsEngine>()?.AddEntityToSimulation(entity);
			entity.CallStart();
		}

		/// <summary>
		/// Removes an entity from the engine.
		/// </summary>
		/// <param name="entity">The entity to remove.</param>
		public static void RemoveEntity(Entity entity)
		{
			Instance.EntityManager.RemoveEntity(entity);
			Instance.GetEngineComponent<IPhysicsEngine>()?.RemoveEntityFromSimulation(entity);
		}
	}

	internal record struct ComponentHolder(IEngineComponent component, bool separateThread);
}
