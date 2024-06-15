using System.Drawing;
using System.Numerics;

namespace Basalt.Types
{
	public struct Particle
	{
		public Vector3 Position = Vector3.Zero;
		public Quaternion Rotation = Quaternion.Identity; 
		public Vector3 Velocity = Vector3.Zero;
		public float Lifetime = 0;
		public Vector3 Scale { get; set; } = Vector3.One;
		public Color Color { get; set; } = Color.HotPink;

        public Particle()
        {
			
        }
        public Particle(Vector3 position, Quaternion rotation)
		{
			Position = position;
			Rotation = rotation;
		}
		public Particle(Vector3 position, Quaternion rotation, Vector3 velocity)
		{
			Position = position;
			Rotation = rotation;
			Velocity = velocity;
		}
		public Particle(Vector3 position, Quaternion rotation, Vector3 velocity, float lifetime)
		{
			Position = position;
			Rotation = rotation;
			Velocity = velocity;
			Lifetime = lifetime;
		}
		public Particle(Vector3 position, Quaternion rotation, Vector3 velocity, float lifetime, Vector3 scale)
		{
			Position = position;
			Rotation = rotation;
			Velocity = velocity;
			Lifetime = lifetime;
			Scale = scale;
		}
		public Particle(Vector3 position, Quaternion rotation, Vector3 velocity, float lifetime, Vector3 scale, Color color)
		{
			Position = position;
			Rotation = rotation;
			Velocity = velocity;
			Lifetime = lifetime;
			Scale = scale;
			Color = color;
		}

	}
}
