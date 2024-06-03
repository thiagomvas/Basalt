
using Basalt.Common.Entities;
using Basalt.Common.Utils;
using Basalt.Core.Common.Abstractions.Engine;
using Newtonsoft.Json;

namespace Basalt.Common.Components
{
	/// <summary>
	/// Represents a base class for components in the Basalt game engine.
	/// </summary>
	public abstract class Component
	{
		/// <summary>
		/// The entity that owns this component.
		/// </summary>
		[JsonIgnore]
		public Entity Entity;

		internal bool started = false;
		private bool destroyed = false;

		public bool Enabled { get; set; } = true;

		/// <summary>
		/// Initializes a new instance of the <see cref="Component"/> class.
		/// </summary>
		/// <param name="entity">The entity that owns this component.</param>
		protected Component(Entity entity)
		{
			this.Entity = entity;
			if (Engine.Instance.Running)
				SubscribeToEvents();
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
		public virtual void OnStart() { }

		/// <summary>
		/// Called every frame to update the component.
		/// </summary>
		public virtual void OnUpdate() { }

		/// <summary>
		/// Called every physics update to update the component's physics.
		/// </summary>
		public virtual void OnPhysicsUpdate()
		{
		}

		/// <summary>
		/// Called whenever the object is colliding with another object.
		/// </summary>
		/// <param name="other"></param>
		public virtual void OnCollision(Collider other)
		{
		}

		internal void onDestroy()
		{
			if (destroyed)
				return;
			destroyed = true;
			UnsubscribeFromEvents();
			OnDestroy();
		}

		/// <summary>
		/// Called when the component is destroyed.
		/// </summary>
		public virtual void OnDestroy()
		{
		}

		public void OnStartEvent(object? sender, EventArgs args)
		{
			if (started)
				return;
			started = true;
			OnStart();
		}

		public void OnUpdateEvent(object? sender, EventArgs args)
		{
			if (Entity.Enabled && Enabled)
				OnUpdate();
		}

		public void OnPhysicsUpdateEvent(object? sender, EventArgs args)
		{
			if (Entity.Enabled && Enabled)
				OnPhysicsUpdate();
		}

		public void OnRenderEvent(object? sender, EventArgs args)
		{
			if (Entity.Enabled && Enabled)
				OnRender();
		}



		private virtual protected void SubscribeToEvents()
		{
			var eventbus = Engine.Instance.GetEngineComponent<IEventBus>()!;
			Type type = this.GetType();

			// Check if OnUpdate was overriden
			if (type.GetMethod(nameof(OnUpdate))!.DeclaringType != typeof(Component))
				eventbus.Subscribe(BasaltConstants.UpdateEventKey, OnUpdateEvent);

			// Check if OnRender was overriden
			if (type.GetMethod(nameof(OnRender))!.DeclaringType != typeof(Component))
				eventbus.Subscribe(BasaltConstants.RenderEventKey, OnRenderEvent);

			// Check if OnPhysicsUpdate was overriden
			if (type.GetMethod(nameof(OnPhysicsUpdate))!.DeclaringType != typeof(Component))
				eventbus.Subscribe(BasaltConstants.PhysicsUpdateEventKey, OnPhysicsUpdateEvent);
		}

		private virtual protected void UnsubscribeFromEvents()
		{
			var eventbus = Engine.Instance.GetEngineComponent<IEventBus>()!;
			eventbus.Unsubscribe(BasaltConstants.StartEventKey, OnStartEvent);
			eventbus.Unsubscribe(BasaltConstants.UpdateEventKey, OnUpdateEvent);
			eventbus.Unsubscribe(BasaltConstants.RenderEventKey, OnRenderEvent);
			eventbus.Unsubscribe(BasaltConstants.PhysicsUpdateEventKey, OnPhysicsUpdateEvent);
		}


	}
}
