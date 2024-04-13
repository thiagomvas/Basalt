using Basalt.Common.Entities;
using System.Numerics;

namespace Basalt.Common.Components
{
	public class FixedLink : Component
	{
		public Entity AnchoredEntity { get; set; }
		public float Distance { get; set; }
		public FixedLink(Entity entity) : base(entity)
		{
		}

		public override void OnStart()
		{

		}

		public override void OnUpdate()
		{
			Vector3 dir = AnchoredEntity.Transform.Position - Entity.Transform.Position;

			var currentDistance = dir.Length();

			Vector3 displacement = Vector3.Normalize(dir) * (currentDistance - Distance);

			Entity.Transform.Position += displacement * 0.5f;
			AnchoredEntity.Transform.Position -= displacement * 0.5f;

			// Check if the entities have Rigidbody components
			Rigidbody? entityRigidbody = Entity.Rigidbody;
			Rigidbody? anchoredEntityRigidbody = AnchoredEntity.Rigidbody;

			if (entityRigidbody != null && anchoredEntityRigidbody != null)
			{
				// Calculate the total mass of the entities
				float totalMass = entityRigidbody.Mass + anchoredEntityRigidbody.Mass;

				// Calculate the velocity adjustment based on the mass ratio
				float velocityAdjustment = displacement.Length() * (entityRigidbody.Mass / totalMass);

				// Remove velocity from the entities
				entityRigidbody.Velocity += displacement * (entityRigidbody.Mass / totalMass);
				anchoredEntityRigidbody.Velocity -= displacement * (anchoredEntityRigidbody.Mass / totalMass);
			}
		}
	}
}
