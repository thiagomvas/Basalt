using Basalt.Common.Entities;
using System.Numerics;

namespace Basalt.Common.Components
{
	public class ChainLink : Component
	{
		public Entity AnchoredEntity { get; set; }
		public float MaxDistance { get; set; }
		public ChainLink(Entity entity) : base(entity)
		{
		}

		public override void OnStart()
		{

		}

		public override void OnUpdate()
		{
			Vector3 dir = AnchoredEntity.Transform.Position - Entity.Transform.Position;

			float distance = dir.LengthSquared();

			if (distance > MaxDistance * MaxDistance)
			{
				Vector3 displacement = Vector3.Normalize(dir) * (MathF.Sqrt(distance) - MaxDistance);

				Entity.Transform.Position += displacement * 0.5f;
				AnchoredEntity.Transform.Position -= displacement * 0.5f;

				if (Entity.Rigidbody != null && AnchoredEntity.Rigidbody != null)
				{
					Vector3 relativeVelocity = Entity.Rigidbody.Velocity - AnchoredEntity.Rigidbody.Velocity;
					float relativeSpeed = Vector3.Dot(relativeVelocity, displacement);

					if (relativeSpeed < 0)
					{
						Vector3 correctionVelocity = -relativeSpeed * displacement;

						Entity.Rigidbody.Velocity += correctionVelocity * 0.5f;
						AnchoredEntity.Rigidbody.Velocity -= correctionVelocity * 0.5f;
					}
				}
			}
		}
	}
}
