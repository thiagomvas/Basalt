using Basalt.Common.Entities;
using Basalt.Types;
using System.Drawing;
using System.Numerics;

namespace Basalt.Common.Components
{
	/// <summary>
	/// A base class for particle system components.
	/// </summary>
	public abstract class BaseParticleSystem : Component
	{
		/// <summary>
		/// The particle pool used by the particle system.
		/// </summary>
		protected Particle[] _particles;

		/// <summary>
		/// The default particle values to be set on reset.
		/// </summary>
		protected Particle defaults;

		private int _length;
		private float _emissionRate = 5, _particleLifetime = 5, _systemLifetime = 0;


		/// <summary>
		/// Gets or sets the emission rate of the particle system.
		/// </summary>
		/// <remarks>
		/// Modifying this value will resize the particle pool.
		/// </remarks>
		public float EmissionRate
		{
			get => _emissionRate;
			set
			{
				_emissionRate = value;
				ResizePool();
			}
		}

		/// <summary>
		/// Gets or sets the lifetime of the particles in the system.
		/// </summary>
		/// <remarks>
		/// Modifying this value will resize the particle pool.
		/// </remarks>
		public float ParticleLifetime
		{
			get => _particleLifetime;
			set
			{
				_particleLifetime = value;
				ResizePool();
			}
		}

		/// <summary>
		/// Gets or sets the duration of the particle system. The system will reset and stop after this duration.
		/// </summary>
		public float SystemDuration { get; set; } = 5f;

		/// <summary>
		/// Gets or sets whether the particle system should loop.
		/// </summary>
		public bool Looping { get; set; } = false;

		/// <summary>
		/// A delegate for updating particles.
		/// </summary>
		/// <param name="particle">The reference to the particle being updated</param>
		public delegate void ParticleUpdateDelegate(ref Particle particle);
		private ParticleUpdateDelegate? _particleUpdate;
		private ParticleUpdateDelegate? _onParticleReset;
		protected BaseParticleSystem(Entity entity) : base(entity)
		{
			ResizePool();
			for (int i = 0; i < _particles.Length; i++)
			{
				_particles[i].Lifetime = ParticleLifetime / _length * i;
			}
			defaults = new(Entity.Transform.Position, Quaternion.Identity, Vector3.Zero, 0);
		}

		/// <summary>
		/// Renders the particles in the system.
		/// </summary>
		protected abstract void RenderParticles();
		public sealed override void OnRender()
		{
			RenderParticles();
		}
		public override void OnUpdate()
		{
			var dt = Time.DeltaTime;
			_systemLifetime += dt;

			if (!Looping && _systemLifetime > SystemDuration + ParticleLifetime)
				return;

			for (int i = 0; i < _particles.Length; i++)
			{
				_particles[i].Lifetime += dt;
				_particles[i].Position += _particles[i].Velocity * dt;
				_particleUpdate?.Invoke(ref _particles[i]);
				if (_particles[i].Lifetime > ParticleLifetime)
				{
					if (!Looping && _systemLifetime > SystemDuration)
					{
						_particles[i] = defaults;
						_particles[i].Color = Color.FromArgb(0x00000000);
						continue;
					}
					_particles[i] = defaults;
					_onParticleReset?.Invoke(ref _particles[i]);
				}
			}
		}

		private void ResizePool()
		{
			int oldLength = _length;
			_length = (int)(EmissionRate * ParticleLifetime);
			var newPool = new Particle[_length];
			if (_length < oldLength)
			{
				for (int i = 0; i < _length; i++)
				{
					newPool[i] = _particles[i];
				}
			}
			else
			{
				for (int i = 0; i < oldLength; i++)
				{
					newPool[i] = _particles[i];
				}
				for (int i = oldLength; i < _length; i++)
				{
					newPool[i] = defaults;
				}
			}
			_particles = newPool;
		}

		/// <summary>
		/// Subscribes a delegate to be called when a particle is updated every frame.
		/// </summary>
		/// <param name="update">The target delegate</param>
		public void SubscribeUpdate(ParticleUpdateDelegate update)
		{
			_particleUpdate += update;
		}

		/// <summary>
		/// Unsubscribes a delegate from being called when a particle is updated every frame.
		/// </summary>
		/// <param name="update">The target delegate</param>
		public void UnsubscribeUpdate(ParticleUpdateDelegate update)
		{
			_particleUpdate -= update;
		}
		/// <summary>
		/// Subscribes a delegate to be called when a particle is reset.
		/// </summary>
		/// <param name="update">The target delegate</param>

		public void SubscribeOnParticleReset(ParticleUpdateDelegate update)
		{
			_onParticleReset += update;
		}

		/// <summary>
		/// Unsubscribes a delegate from being called when a particle is reset.
		/// </summary>
		/// <param name="update">The target delegate</param>
		public void UnsubscribeOnParticleReset(ParticleUpdateDelegate update)
		{
			_onParticleReset -= update;
		}

		/// <summary>
		/// Changes the default values of the particles in the system.
		/// </summary>
		/// <param name="particle">The new default particle value</param>
		public void UpdateDefaults(Particle particle)
		{
			particle.Position = Entity.Transform.Position;
			defaults = particle;
		}

		/// <summary>
		/// Resets the particle system to it's initial state.
		/// </summary>
		public void Reset()
		{
			_systemLifetime = 0;
			for (int i = 0; i < _particles.Length; i++)
			{
				_particles[i] = defaults;
				_particles[i].Lifetime = ParticleLifetime / _length * i;
			}
		}


		/// <summary>
		/// Stop the particle system from emitting new particles. Already existing particles will continue to update until end of lifetime.
		/// </summary>
		public void Stop()
		{
			_systemLifetime = SystemDuration;
		}

	}
}
