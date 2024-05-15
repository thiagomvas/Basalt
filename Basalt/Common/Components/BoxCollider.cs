using Basalt.Common.Entities;
using Newtonsoft.Json;
using System.Numerics;

namespace Basalt.Common.Components
{
	/// <summary>
	/// Represents a box collider component that can be attached to an entity.
	/// </summary>
	public class BoxCollider : Collider
	{
		/// <summary>
		/// The size of the box collider.
		/// </summary>
		public Vector3 Size { get; set; } = Vector3.One;

		/// <summary>
		/// The position of the box collider.
		/// </summary>
		[JsonIgnore]
		private Vector3 pos => Entity.Transform.Position + Offset;

		/// <summary>
		/// Initializes a new instance of the <see cref="BoxCollider"/> class.
		/// </summary>
		/// <param name="entity">The entity to attach the collider to.</param>
		public BoxCollider(Entity entity) : base(entity)
		{

		}

		/// <summary>
		/// Called when the component starts.
		/// </summary>
		public override void OnStart()
		{
		}

		/// <summary>
		/// Called every frame to update the component.
		/// </summary>
		public override void OnUpdate() { }

		/// <summary>
		/// Called when the component needs to be rendered.
		/// </summary>
		public override void OnRender()
		{
			
		}
	}

}
