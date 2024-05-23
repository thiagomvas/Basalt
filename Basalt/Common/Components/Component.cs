
using Basalt.Common.Entities;
using Basalt.Common.Utils;
using Basalt.Core.Common.Abstractions.Engine;
using Newtonsoft.Json;

namespace Basalt.Common.Components
{
	/// <summary>
	/// Represents a base class for components in the Basalt game engine.
	/// </summary>
	public abstract class Component : IObserver
	{
		/// <summary>
		/// The entity that owns this component.
		/// </summary>
		[JsonIgnore]
		public Entity Entity;

		public bool Enabled { get; set; } = true;

		/// <summary>
		/// Initializes a new instance of the <see cref="Component"/> class.
		/// </summary>
		/// <param name="entity">The entity that owns this component.</param>
		protected Component(Entity entity)
		{
			this.Entity = entity;
			SubscribeToEvents();
			if (Engine.Instance.Running)
				OnStartEvent();
		}

		/// <summary>
		/// Called when the component needs to be rendered.
		/// </summary>
		public virtual void OnRender()
		{
		}

		/// <summary>
		/// Called when the component is started.
		/// </summary>
		public abstract void OnStart();

		/// <summary>
		/// Called every frame to update the component.
		/// </summary>
		public abstract void OnUpdate();

		/// <summary>
		/// Called every physics update to update the component's physics.
		/// </summary>
		public virtual void OnPhysicsUpdate()
		{
		}

		public virtual void OnCollision(Collider other)
		{
		}

		internal void onDestroy()
		{
			Engine.Instance.GetEngineComponent<IEventBus>()?.Unsubscribe(this);
			OnDestroy();
		}

		/// <summary>
		/// Called when the component is destroyed.
		/// </summary>
		public virtual void OnDestroy()
		{
		}

		public void OnStartEvent() => OnStart();

		public void OnUpdateEvent()
		{
			if(Entity.Enabled && Enabled)
				OnUpdate();
		}

		public void OnPhysicsUpdateEvent()
		{
			if(Entity.Enabled && Enabled)
				OnPhysicsUpdate();
		}

		public void OnRenderEvent()
		{
			if(Entity.Enabled && Enabled)
				OnRender();
		}

		private void SubscribeToEvents()
		{
			var eventbus = Engine.Instance.GetEngineComponent<IEventBus>()!;
			Type type = this.GetType();

			eventbus.Subscribe(BasaltConstants.StartEventKey, (_, _) => OnStartEvent());
			eventbus.Subscribe(BasaltConstants.UpdateEventKey, (_, _) => OnUpdateEvent());

			// Check if OnRender was overriden
			if (type.GetMethod(nameof(OnRender)).DeclaringType != typeof(Component))
				eventbus.Subscribe(BasaltConstants.RenderEventKey, (_, _) => OnRenderEvent());

			// Check if OnPhysicsUpdate was overriden
			if (type.GetMethod(nameof(OnPhysicsUpdate)).DeclaringType != typeof(Component))
				eventbus.Subscribe(BasaltConstants.PhysicsUpdateEventKey, (_, _) => OnPhysicsUpdateEvent());
		}


	}
}
