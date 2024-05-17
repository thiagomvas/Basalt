using Basalt.Common.Entities;
using System.Numerics;

namespace Basalt.Common.Components
{
	public class FixedLink : Component
	{
		public Entity AnchoredEntity { get; set; }
		public float Distance { get; set; }
		public float JointForceMultiplier { get; set; } = 1.0f;
		public FixedLink(Entity entity) : base(entity)
		{
		}

		public override void OnStart()
		{

		}

		public override void OnUpdate()
		{
			// Optional: Add any behavior you want to occur during regular updates
		}

		public override void OnPhysicsUpdate()
		{
			// Calculate the vector between the two entities
			Vector3 anchorToEntity = AnchoredEntity.Transform.Position - Entity.Transform.Position;

			// Calculate the current distance between the entities
			float currentDistance = anchorToEntity.Length();

			// Calculate the difference in distance from the desired distance
			float difference = currentDistance - Distance;

			// If the difference is positive, entities are too close, if negative, too far
			if (MathF.Abs(difference) > 0.01f) // Add a small tolerance to prevent oscillations
			{
				// Normalize the vector
				Vector3 forceDirection = Vector3.Normalize(anchorToEntity);

				// Calculate the force magnitude based on mass
				float forceMagnitude = difference * (Entity.Rigidbody.Mass + AnchoredEntity.Rigidbody.Mass) * JointForceMultiplier;
				// Apply force to both entities in the correct direction to maintain the fixed distance
				Vector3 force = forceDirection * forceMagnitude;
				Entity.Rigidbody.AddForce(force);
				AnchoredEntity.Rigidbody.AddForce(-force); // Apply equal and opposite force
			}
		}
	}
}
