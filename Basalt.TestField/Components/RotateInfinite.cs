using Basalt.Common;
using Basalt.Common.Components;
using Basalt.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.TestField.Components
{
	public class RotateInfinite : Component
	{
		private float rotationSpeed = 30f; // Rotation speed in degrees per second

		public RotateInfinite(Entity entity) : base(entity)
		{
		}

		public override void OnStart()
		{
		}

		public override void OnUpdate()
		{
			// Calculate the rotation angle based on the elapsed time
			float deltaTime = Time.DeltaTime;
			float angleInDegrees = rotationSpeed * deltaTime;

			// Convert the angle to radians
			float angleInRadians = MathF.PI / 180 * angleInDegrees;

			// Create the rotation quaternion
			Quaternion rotationQuaternion = Quaternion.CreateFromAxisAngle(Vector3.One, angleInRadians);

			// Apply rotation to the entity's quaternion
			Quaternion rotatedQuaternion = Quaternion.Concatenate(Entity.Transform.Rotation, rotationQuaternion);

			// Update the entity's rotation
			Entity.Transform.Rotation = rotatedQuaternion;
		}
	}
}
