using Basalt.Common.Entities;
using Basalt.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Common.Components
{
	public class BoxCollider : Collider
	{
		public Vector3 Size = Vector3.One, Offset = Vector3.Zero;
		public BoxCollider(Entity entity) : base(entity)
		{

		}

		public override void OnStart()
		{
		}

		public override void OnUpdate() { }

		public override void HandleCollision(Collider col)
		{
			switch (col.Type)
			{
				case ColliderType.Box:
					HandleBoxCol((BoxCollider)col);
					break;
				case ColliderType.Sphere:
					Engine.Instance.logger?.LogInformation("Sphere Collider");
					break;
				default:
					Engine.Instance.logger?.LogInformation("Unknown Collider");
					break;
			}
		}

		public override bool Intersects(Collider col)
		{
			switch (col.Type)
			{
				case ColliderType.Box:
					return IntersectsBox((BoxCollider)col);
				case ColliderType.Sphere:
					return false;
				default:
					return false;
			}
		}

		private bool IntersectsBox(BoxCollider col)
		{
			Vector3 extents = Size / 2;
			Vector3 extents2 = col.Size / 2;

			Vector3 min1 = entity.Transform.Position - extents;
			Vector3 max1 = entity.Transform.Position + extents;

			Vector3 min2 = col.entity.Transform.Position - extents2;
			Vector3 max2 = col.entity.Transform.Position + extents2;

			bool intersectsX = min1.X <= max2.X && max1.X >= min2.X;
			bool intersectsY = min1.Y <= max2.Y && max1.Y >= min2.Y;
			bool intersectsZ = min1.Z <= max2.Z && max1.Z >= min2.Z;

			return intersectsX && intersectsY && intersectsZ;
		}

		private void HandleBoxCol(BoxCollider col)
		{// Calculate the extents (half sizes) of the two colliders
			Vector3 extents1 = Size / 2f;
			Vector3 extents2 = col.Size / 2f;

			// Calculate the min and max points of the two colliders along each axis
			Vector3 min1 = entity.Transform.Position - extents1;
			Vector3 max1 = entity.Transform.Position + extents1;
			Vector3 min2 = col.entity.Transform.Position - extents2;
			Vector3 max2 = col.entity.Transform.Position + extents2;

			// Calculate the overlap along each axis
			float overlapX = Math.Max(0, Math.Min(max1.X, max2.X) - Math.Max(min1.X, min2.X));
			float overlapY = Math.Max(0, Math.Min(max1.Y, max2.Y) - Math.Max(min1.Y, min2.Y));
			float overlapZ = Math.Max(0, Math.Min(max1.Z, max2.Z) - Math.Max(min1.Z, min2.Z));

			// Calculate the direction of least penetration
			Vector3 separationDirection = Vector3.Zero;
			float minOverlap = Math.Min(overlapX, Math.Min(overlapY, overlapZ));
			if (minOverlap == overlapX)
			{
				separationDirection = (entity.Transform.Position.X < col.entity.Transform.Position.X) ? -Vector3.UnitX : Vector3.UnitX;
			}
			else if (minOverlap == overlapY)
			{
				separationDirection = (entity.Transform.Position.Y < col.entity.Transform.Position.Y) ? -Vector3.UnitY : Vector3.UnitY;
			}
			else if (minOverlap == overlapZ)
			{
				separationDirection = (entity.Transform.Position.Z < col.entity.Transform.Position.Z) ? -Vector3.UnitZ : Vector3.UnitZ;
			}

			// Move the colliders to separate them along the direction of least penetration
			float separationDistance = Math.Abs(minOverlap);
			entity.Transform.Position += separationDirection * separationDistance / 2f;
			col.entity.Transform.Position -= separationDirection * separationDistance / 2f;

			// Now position1 and position2 are updated to resolve the collision
		}
	}

}
