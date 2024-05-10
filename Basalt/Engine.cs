using Basalt.Common.Entities;
using Basalt.Core.Common.Abstractions.Engine;
using System.Diagnostics;

namespace Basalt
{
	public class Engine
	{
		#region Singleton
		private static Engine? _instance;
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

		internal Engine()
		{
			_instance = this;
		}

		#endregion

		public bool Running { get; private set; } = false;
		private Dictionary<Type, ComponentHolder> Components { get; set; } = new();
		public EntityManager EntityManager;
		private ILogger? _logger;
		private List<Entity> queuedEntities = new();

		public CountdownEvent CountdownEvent { get; } = new(1);

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

		public void Initialize()
		{
			// Block initialization if no graphics engine or event bus is found
			if(!Components.ContainsKey(typeof(IGraphicsEngine)))
			{
				Logger?.LogFatal("Could not find a Graphics Engine component that implements IGraphicsEngine. Cannot run without one.");
				return;
			}

			if(!Components.ContainsKey(typeof(IEventBus)))
			{
				Logger?.LogFatal("Could not find an Event Bus component that implements IEventBus. Cannot run without one.");
				return;
			}

			Running = true;

			// Initialize Entity Manager
			EntityManager = new(GetEngineComponent<IEventBus>());
			foreach(var e in queuedEntities)
			{
				CreateEntity(e);
			}

			// Move graphics engine to the front of the list
			Components = Components.OrderBy(c => c.Key == typeof(IGraphicsEngine) ? 0 : 1).ToDictionary(c => c.Key, c => c.Value);

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

			Instance.GetEngineComponent<IEventBus>()?.NotifyStart();
		}

		public void Shutdown()
		{
			Running = false;
			Logger?.LogWarning("Shutting down engine...");
			foreach (var component in Components)
			{
				component.Value.component.Shutdown();
			}
		}

		public static void CreateEntity(Entity entity)
		{
			if(!Instance.Running)
			{
				Instance.queuedEntities.Add(entity);
				return;
			}
			Instance.Logger?.LogDebug($"Creating entity {entity.Id}...");
			Instance.EntityManager.AddEntity(entity);
		}

		public static void RemoveEntity(Entity entity)
		{
			Instance.EntityManager.RemoveEntity(entity);
		}
	}

	internal record struct ComponentHolder(IEngineComponent component, bool separateThread);
}
