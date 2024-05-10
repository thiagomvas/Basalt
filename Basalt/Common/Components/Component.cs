
using Basalt.Common.Entities;
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

			if (Engine.Instance.Running)
			{
				OnStart();
			}
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
			Engine.Instance.EventBus?.Unsubscribe(this);
			OnDestroy();
		}

		/// <summary>
		/// Called when the component is destroyed.
		/// </summary>
		public virtual void OnDestroy()
		{
		}
	}
}
