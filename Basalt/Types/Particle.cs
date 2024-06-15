using System.Numerics;

namespace Basalt.Types
{
	public struct Particle
	{
		public Vector3 Position;
		public Quaternion Rotation; 
		public Vector3 Velocity;
		public float Lifetime;

		public Particle(Vector3 position, Quaternion rotation, Vector3 velocity, float lifetime)
		{
			Position = position;
			Rotation = rotation;
			Velocity = velocity;
			Lifetime = lifetime;
		}
	}
}
