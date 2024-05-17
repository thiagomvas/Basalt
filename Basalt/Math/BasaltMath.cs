using System.Numerics;

namespace Basalt.Math
{
	public static class BasaltMaWth
	{
		/// <summary>
		/// Scales a value from one range to another.
		/// </summary>
		/// <param name="value">The value to scale.</param>
		/// <param name="min">The minimum value of the input range.</param>
		/// <param name="max">The maximum value of the input range.</param>
		/// <param name="newMin">The minimum value of the output range.</param>
		/// <param name="newMax">The maximum value of the output range.</param>
		/// <returns>The scaled value.</returns>
		public static float Scale(float value, float min, float max, float newMin, float newMax)
		{
			return (value - min) / (max - min) * (newMax - newMin) + newMin;
		}

		/// <summary>
		/// Returns the rotation from the origin if it was looking at a target.
		/// </summary>
		/// <param name="origin">The origin coordinates.</param>
		/// <param name="target">The target's position to rotate towards.</param>
		/// <param name="up">The up vector from the origin.</param>
		/// <returns>A rotation representing the direction an object is looking at.</returns>
		public static Quaternion LookAtRotation(Vector3 origin, Vector3 target, Vector3 up)
		{
			Vector3 forward = Vector3.Normalize(target - origin);
			Vector3 right = Vector3.Normalize(Vector3.Cross(up, forward));
			Vector3 newUp = Vector3.Cross(forward, right);

			Matrix4x4 matrix = new Matrix4x4(
				right.X, right.Y, right.Z, 0,
				newUp.X, newUp.Y, newUp.Z, 0,
				forward.X, forward.Y, forward.Z, 0,
				0, 0, 0, 1
			);

			return Quaternion.CreateFromRotationMatrix(matrix);
		}

		/// <summary>
		/// Gets the forward vector of a rotation.
		/// </summary>
		/// <param name="rotation">The rotation.</param>
		/// <returns>The forward vector.</returns>
		public static Vector3 GetForwardVector(Quaternion rotation)
		{
			Vector3 rotatedForward = Vector3.Normalize(Vector3.Transform(Vector3.UnitZ, Quaternion.Normalize(rotation)));
			return rotatedForward;
		}

		/// <summary>
		/// Gets the right vector of a rotation.
		/// </summary>
		/// <param name="quaternion">The rotation.</param>
		/// <returns>The right vector.</returns>
		public static Vector3 GetRightVector(Quaternion quaternion)
		{
			// Apply the quaternion rotation to the right vector (1, 0, 0)
			Vector3 rightVector = Vector3.Transform(new Vector3(-1, 0, 0), quaternion);
			return rightVector;
		}

		/// <summary>
		/// Rotates a vector by the variation between two quaternions.
		/// </summary>
		/// <param name="vector">The vector to rotate.</param>
		/// <param name="startRotation">The starting rotation.</param>
		/// <param name="finalRotation">The final rotation.</param>
		/// <returns>The rotated vector.</returns>
		public static Vector3 RotateVectorByQuaternionVariation(Vector3 vector, Quaternion startRotation, Quaternion finalRotation)
		{
			// Convert quaternions to rotation matrices
			Matrix4x4 startRotationMatrix = Matrix4x4.CreateFromQuaternion(startRotation);
			Matrix4x4 finalRotationMatrix = Matrix4x4.CreateFromQuaternion(finalRotation);

			// Calculate the variation in rotation
			Matrix4x4 rotationVariationMatrix = finalRotationMatrix * Matrix4x4.Transpose(startRotationMatrix);

			// Rotate the vector by the rotation variation
			Vector3 rotatedVector = Vector3.Transform(vector, rotationVariationMatrix);

			return rotatedVector;
		}

		/// <summary>
		/// Calculates the angle between two vectors.
		/// </summary>
		/// <param name="vector1">The first vector.</param>
		/// <param name="vector2">The second vector.</param>
		/// <returns>The angle between the two vectors in radians.</returns>
		public static float AngleBetweenVectors(Vector3 vector1, Vector3 vector2)
		{
			// Calculate the dot product
			float dotProduct = Vector3.Dot(vector1, vector2);

			// Normalize the vectors
			float magnitude1 = vector1.Length();
			float magnitude2 = vector2.Length();

			// Calculate the angle in radians
			float angle = (float)System.Math.Acos(dotProduct / (magnitude1 * magnitude2));
			return angle;
		}
	}
}
