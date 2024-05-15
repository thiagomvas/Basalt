using Basalt.Core.Common.Abstractions.Engine;

namespace Basalt.Common
{
	/// <summary>
	/// Represents a builder class for constructing an <see cref="Engine"/>.
	/// </summary>
	public class EngineBuilder
	{
		Dictionary<Type, ComponentHolder> _components = new();
		private record struct ComponentHolder(Type implementation, Func<IEngineComponent> initFunc, bool separateThread = false);

		ILogger? logger;

		/// <summary>
		/// Adds a logger to the engine builder.
		/// </summary>
		/// <param name="logger">The logger to add.</param>
		/// <returns>The modified engine builder instance.</returns>
		public EngineBuilder AddLogger(ILogger logger)
		{
			this.logger = logger;
			return this;
		}

		/// <summary>
		/// Adds an engine component to the builder.
		/// </summary>
		/// <typeparam name="TType">The type of the component to add.</typeparam>
		/// <param name="init">The function to initialize the component.</param>
		/// <param name="separateThread">Indicates whether the component should run on a separate thread.</param>
		/// <returns>The modified engine builder instance.</returns>
		public EngineBuilder AddComponent<TType>(Func<TType> init, bool separateThread = false) where TType : IEngineComponent
		{
			_components.Add(typeof(TType), new ComponentHolder(typeof(TType), () => init(), separateThread));
			return this;
		}

		/// <summary>
		/// Adds an engine component to the builder.
		/// </summary>
		/// <typeparam name="TType">The type of the component to add.</typeparam>
		/// <param name="separateThread">Indicates whether the component should run on a separate thread.</param>
		/// <returns>The modified engine builder instance.</returns>
		public EngineBuilder AddComponent<TType>(bool separateThread = false) where TType : IEngineComponent
		{
			_components.Add(typeof(TType), new ComponentHolder(typeof(TType), () => Activator.CreateInstance<TType>(), separateThread));
			return this;
		}


		/// <summary>
		/// Adds an engine component to the builder with a specific implementation type.
		/// </summary>
		/// <typeparam name="TType">The type of the component interface.</typeparam>
		/// <typeparam name="TImpl">The type of the component implementation.</typeparam>
		/// <param name="init">The function to initialize the component.</param>
		/// <param name="separateThread">Indicates whether the component should run on a separate thread.</param>
		/// <returns>The modified engine builder instance.</returns>
		public EngineBuilder AddComponent<TType, TImpl>(Func<TImpl> init, bool separateThread = false) 
			where TImpl : TType
			where TType : IEngineComponent
		{
			_components.Add(typeof(TType), new ComponentHolder(typeof(TImpl), () => init(), separateThread));
			return this;
		}


		/// <summary>
		/// Adds an engine component to the builder with a specific implementation type.
		/// </summary>
		/// <typeparam name="TType">The type of the component interface.</typeparam>
		/// <typeparam name="TImpl">The type of the component implementation.</typeparam>
		/// <param name="separateThread">Indicates whether the component should run on a separate thread.</param>
		/// <returns>The modified engine builder instance.</returns>
		public EngineBuilder AddComponent<TType, TImpl>(bool separateThread = false )
			
			where TImpl : TType
			where TType : IEngineComponent
		{
			_components.Add(typeof(TType), new ComponentHolder(typeof(TImpl), () => Activator.CreateInstance<TImpl>(), separateThread));
			return this;
		}


		/// <summary>
		/// Builds the engine based on the components added to the builder.
		/// </summary>
		/// <returns>The constructed engine.</returns>
		public Engine Build()
		{
			var engine = new Engine();
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
