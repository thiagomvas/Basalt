using Basalt.Core.Common.Abstractions.Engine;
using System.Diagnostics;

namespace Basalt
{
	public class Engine2
	{
		public bool Running { get; private set; } = false;
		private Dictionary<Type, ComponentHolder> Components { get; set; } = new();

		private ILogger? _logger;

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
		}
	}

	internal record struct ComponentHolder(IEngineComponent component, bool separateThread);
}
