using Basalt.Common.Components;
using System.Drawing;
using System.Numerics;

namespace Basalt.Types
{
	/// <summary>
	/// A struct representing a particle used by <see cref="BaseParticleSystem"/> derived types.
	/// </summary>
	public struct Particle
	{
		/// <summary>
		/// The current position of the particle.
		/// </summary>
		public Vector3 Position = Vector3.Zero;

		/// <summary>
		/// The rotation of the particle.
		/// </summary>
		public Quaternion Rotation = Quaternion.Identity;

		/// <summary>
		/// The velocity of the particle.
		/// </summary>
		public Vector3 Velocity = Vector3.Zero;

		/// <summary>
		/// The lifetime of the particle.
		/// </summary>
		public float Lifetime = 0;

		/// <summary>
		/// The scale of the particle.
		/// </summary>
		public Vector3 Scale { get; set; } = Vector3.One;

		/// <summary>
		/// The color of the particle.
		/// </summary>
		public Color Color { get; set; } = Color.HotPink;

		/// <summary>
		/// Initializes a new instance of the <see cref="Particle"/> struct.
		/// </summary>
		public Particle()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Particle"/> struct with the specified position and rotation.
		/// </summary>
		/// <param name="position">The position of the particle.</param>
		/// <param name="rotation">The rotation of the particle.</param>
		public Particle(Vector3 position, Quaternion rotation)
		{
			Position = position;
			Rotation = rotation;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Particle"/> struct with the specified position, rotation, and velocity.
		/// </summary>
		/// <param name="position">The position of the particle.</param>
		/// <param name="rotation">The rotation of the particle.</param>
		/// <param name="velocity">The velocity of the particle.</param>
		public Particle(Vector3 position, Quaternion rotation, Vector3 velocity)
		{
			Position = position;
			Rotation = rotation;
			Velocity = velocity;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Particle"/> struct with the specified position, rotation, velocity, and lifetime.
		/// </summary>
		/// <param name="position">The position of the particle.</param>
		/// <param name="rotation">The rotation of the particle.</param>
		/// <param name="velocity">The velocity of the particle.</param>
		/// <param name="lifetime">The lifetime of the particle.</param>
		public Particle(Vector3 position, Quaternion rotation, Vector3 velocity, float lifetime)
		{
			Position = position;
			Rotation = rotation;
			Velocity = velocity;
			Lifetime = lifetime;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Particle"/> struct with the specified position, rotation, velocity, lifetime, and scale.
		/// </summary>
		/// <param name="position">The position of the particle.</param>
		/// <param name="rotation">The rotation of the particle.</param>
		/// <param name="velocity">The velocity of the particle.</param>
		/// <param name="lifetime">The lifetime of the particle.</param>
		/// <param name="scale">The scale of the particle.</param>
		public Particle(Vector3 position, Quaternion rotation, Vector3 velocity, float lifetime, Vector3 scale)
		{
			Position = position;
			Rotation = rotation;
			Velocity = velocity;
			Lifetime = lifetime;
			Scale = scale;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Particle"/> struct with the specified position, rotation, velocity, lifetime, scale, and color.
		/// </summary>
		/// <param name="position">The position of the particle.</param>
		/// <param name="rotation">The rotation of the particle.</param>
		/// <param name="velocity">The velocity of the particle.</param>
		/// <param name="lifetime">The lifetime of the particle.</param>
		/// <param name="scale">The scale of the particle.</param>
		/// <param name="color">The color of the particle.</param>
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
