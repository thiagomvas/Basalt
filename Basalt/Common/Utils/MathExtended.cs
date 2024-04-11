using System.Numerics;

namespace Basalt.Common.Utils
{
	public static class MathExtended
	{
		/// <summary>
		/// Returns the rotation from the origin if it was looking at a target
		/// </summary>
		/// <param name="origin">The origin coordinates</param>
		/// <param name="target">The target's position to rotate towards</param>
		/// <param name="up">The up vector from the origin</param>
		/// <returns>A rotation representing the direction an object is looking at</returns>
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
		/// <param name="rotation">The rotation</param>
		/// <returns>The forward vector</returns>
		public static Vector3 GetForwardVector(Quaternion rotation)
		{
			Vector3 rotatedForward = Vector3.Normalize(Vector3.Transform(Vector3.UnitZ, Quaternion.Normalize(rotation)));
			return rotatedForward;
		}

		public static Vector3 GetRightVector(Quaternion quaternion)
		{
			// Apply the quaternion rotation to the right vector (1, 0, 0)
			Vector3 rightVector = Vector3.Transform(new Vector3(-1, 0, 0), quaternion);
			return rightVector;
		}

	}
}
