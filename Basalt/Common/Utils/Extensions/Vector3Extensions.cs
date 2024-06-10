using System.Numerics;

namespace Basalt.Common.Utils.Extensions
{
	public static class Vector3Extensions
	{
		/// <summary>
		/// Normalizes the vector in the XZ plane.
		/// </summary>
		/// <param name="vector">The vector to normalize.</param>
		/// <returns>The normalized vector.</returns>
		public static Vector3 XZNormalized(this Vector3 vector)
		{
			return Vector3.Normalize(new Vector3(vector.X, 0, vector.Z));
		}

		/// <summary>
		/// Normalizes the vector in the YZ plane.
		/// </summary>
		/// <param name="vector">The vector to normalize.</param>
		/// <returns>The normalized vector.</returns>
		public static Vector3 YZNormalized(this Vector3 vector)
		{
			return Vector3.Normalize(new Vector3(0, vector.Y, vector.Z));
		}

		/// <summary>
		/// Normalizes the vector in the XY plane.
		/// </summary>
		/// <param name="vector">The vector to normalize.</param>
		/// <returns>The normalized vector.</returns>
		public static Vector3 XYNormalized(this Vector3 vector)
		{
			return Vector3.Normalize(new Vector3(vector.X, vector.Y, 0));
		}

		/// <summary>
		/// Returns a new vector with the Z component set to 0.
		/// </summary>
		/// <param name="vector">The original vector.</param>
		/// <returns>A new vector with the Z component set to 0.</returns>
		public static Vector3 XY(this Vector3 vector) => new Vector3(vector.X, vector.Y, 0);

		/// <summary>
		/// Returns a new vector with the Y component set to 0.
		/// </summary>
		/// <param name="vector">The original vector.</param>
		/// <returns>A new vector with the Y component set to 0.</returns>
		public static Vector3 XZ(this Vector3 vector) => new Vector3(vector.X, 0, vector.Z);

		/// <summary>
		/// Returns a new vector with the X component set to 0.
		/// </summary>
		/// <param name="vector">The original vector.</param>
		/// <returns>A new vector with the X component set to 0.</returns>
		public static Vector3 YZ(this Vector3 vector) => new Vector3(0, vector.Y, vector.Z);
	}
}
