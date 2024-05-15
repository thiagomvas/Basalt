using Basalt.Common.Entities;
using System.Numerics;

namespace Basalt.Common.Components
{
	/// <summary>
	/// Represents an abstract base class for colliders.
	/// </summary>
	public abstract class Collider : Component
	{
		/// <summary>
		/// Gets or sets the offset of the collider.
		/// </summary>
		public Vector3 Offset;

		/// <summary>
		/// Gets the position of the collider relative to the entity's transform.
		/// </summary>
		public Vector3 Position => Entity.Transform.Position + Offset;

		/// <summary>
		/// Initializes a new instance of the <see cref="Collider"/> class.
		/// </summary>
		/// <param name="entity">The entity that the collider belongs to.</param>
		protected Collider(Entity entity) : base(entity)
		{
		}

		/// <summary>
		/// Called when the collider has collided with another collider or is still colliding.
		/// </summary>
		/// <param name="other"></param>
		internal void InternalOnCollision(Collider other) => Entity.CallOnCollision(other);

	}
}
