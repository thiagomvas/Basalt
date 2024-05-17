using System.Numerics;

namespace Basalt.Tests.Common
{
	internal class Vector3EqualityComparer : EqualityComparer<Vector3>
	{
		private readonly float delta;

		public Vector3EqualityComparer(float delta = 0.0001f)
		{
			this.delta = delta;
		}

		public override bool Equals(Vector3 x, Vector3 y)
		{
			return System.Math.Abs(x.X - y.X) < delta &&
				   System.Math.Abs(x.Y - y.Y) < delta &&
				   System.Math.Abs(x.Z - y.Z) < delta;
		}

		public override int GetHashCode(Vector3 obj)
		{
			return obj.GetHashCode();
		}

	}
}
