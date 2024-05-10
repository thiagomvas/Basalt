using Basalt.Core.Common.Abstractions.Engine;

namespace Basalt.Common
{
	public class EngineBuilder
	{
		Dictionary<Type, ComponentHolder> _components = new();
		private record struct ComponentHolder(Type implementation, Func<IEngineComponent> initFunc, bool separateThread = false);

		ILogger? logger;

		public EngineBuilder AddLogger(ILogger logger)
		{
			this.logger = logger;
			return this;
		}

		public EngineBuilder AddComponent<TType, TImpl>(Func<TImpl> init, bool separateThread = false) 
			where TImpl : TType
			where TType : IEngineComponent
		{
			_components.Add(typeof(TType), new(typeof(TImpl), () => init(), separateThread));
			return this;
		}

		public EngineBuilder AddComponent<TType, TImpl>(bool separateThread = false )
			
			where TImpl : TType
			where TType : IEngineComponent
		{
			_components.Add(typeof(TType), new(typeof(TImpl), () => Activator.CreateInstance<TImpl>(), separateThread));
			return this;
		}
		public Engine2 Build()
		{
			var engine = new Engine2();
			foreach (var component in _components)
			{
				engine.AddComponent(component.Key, component.Value.initFunc(), component.Value.separateThread);
			}

			if (logger != null)
			{
				engine.AddLogger(logger);
			}

			logger?.LogDebug($"Successfully built engine containing {_components.Count} components: {string.Join(",", _components.Values.Select(c => c.implementation.Name))}");
			return engine;
		}

	}
}
