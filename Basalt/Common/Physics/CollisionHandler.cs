using Basalt.Common.Components;
using Basalt.Common.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Common.Physics
{
	public static class CollisionHandler
	{
		private delegate void CollisionHandlerDelegate(Collider col1, Collider col2);
		private readonly static Dictionary<(Type, Type), CollisionHandlerDelegate> handlers = new()
		{
			{(typeof(BoxCollider), typeof(BoxCollider)), BoxBoxCollision},
		};

		public static void Handle(Collider col1, Collider col2)
		{
			if (handlers.TryGetValue((col1.GetType(), col2.GetType()), out CollisionHandlerDelegate handler))
			{
				handler(col1, col2);
			}

			else
			{
				Engine.Instance.logger?.LogError($"No collision handler for {col1.GetType()} and {col2.GetType()}");
			}
		}

		private static void BoxBoxCollision(Collider col1, Collider col2)
		{
			BoxCollider box1 = (BoxCollider)col1;
			BoxCollider box2 = (BoxCollider)col2;


			Vector3 extents1 = box1.Size / 2f;
			Vector3 extents2 = box2.Size / 2f;

			// Calculate the min and max points of the two colliders along each axis
			Vector3 min1 = box1.Entity.Transform.Position - extents1;
			Vector3 max1 = box1.Entity.Transform.Position + extents1;
			Vector3 min2 = box2.Entity.Transform.Position - extents2;
			Vector3 max2 = box2.Entity.Transform.Position + extents2;

			// Calculate the overlap along each axis
			float overlapX = Math.Max(0, Math.Min(max1.X, max2.X) - Math.Max(min1.X, min2.X));
			float overlapY = Math.Max(0, Math.Min(max1.Y, max2.Y) - Math.Max(min1.Y, min2.Y));
			float overlapZ = Math.Max(0, Math.Min(max1.Z, max2.Z) - Math.Max(min1.Z, min2.Z));

			// Calculate the direction of least penetration
			Vector3 separationDirection = Vector3.Zero;
			float minOverlap = Math.Min(overlapX, Math.Min(overlapY, overlapZ));
			if (minOverlap == overlapX)
			{
				separationDirection = (box1.Entity.Transform.Position.X < box2.Entity.Transform.Position.X) ? -Vector3.UnitX : Vector3.UnitX;
			}
			else if (minOverlap == overlapY)
			{
				separationDirection = (box1.Entity.Transform.Position.Y < box2.Entity.Transform.Position.Y) ? -Vector3.UnitY : Vector3.UnitY;
			}
			else if (minOverlap == overlapZ)
			{
				separationDirection = (box1.Entity.Transform.Position.Z < box2.Entity.Transform.Position.Z) ? -Vector3.UnitZ : Vector3.UnitZ;
			}

			// Move the colliders to separate them along the direction of least penetration
			float separationDistance = Math.Abs(minOverlap);
			box1.Entity.Transform.Position += separationDirection * separationDistance / 2f;
			box2.Entity.Transform.Position -= separationDirection * separationDistance / 2f;
		}
	}
}
