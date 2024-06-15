using Basalt.Common.Entities;
using Basalt.Types;
using System.Numerics;

namespace Basalt.Common.Components
{
	public abstract class BaseParticleSystem : Component
	{

		protected Particle[] _particles;

		private Particle defaults;
		private int _length;
		private float _emissionRate = 5, _particleLifetime = 5;
		public float EmissionRate
		{
			get => _emissionRate;
			set
			{ 
				_emissionRate = value;
				ResizePool();
			}
		}

		public float ParticleLifetime
		{
			get => _particleLifetime;
			set
			{
				_particleLifetime = value;
				ResizePool();
			}
		}

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

		public abstract void RenderParticles();
		public sealed override void OnRender()
		{
			RenderParticles();
		}
		public override void OnUpdate()
		{
			var dt = Time.DeltaTime;
			for (int i = 0; i < _particles.Length; i++)
			{
				_particles[i].Lifetime += dt;
				_particles[i].Position += _particles[i].Velocity * dt;
				_particleUpdate?.Invoke(ref _particles[i]);
				if (_particles[i].Lifetime > ParticleLifetime)
				{
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
			if(_length < oldLength)
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
					newPool[i] = new Particle(Entity.Transform.Position, Quaternion.Identity, Vector3.One, 0);
				}
			}
			_particles = newPool;
		}

		public void SubscribeUpdate(ParticleUpdateDelegate update)
		{
			_particleUpdate += update;
		}

		public void UnsubscribeUpdate(ParticleUpdateDelegate update)
		{
			_particleUpdate -= update;
		}

		public void SubscribeOnParticleReset(ParticleUpdateDelegate update)
		{
			_onParticleReset += update;
		}

		public void UnsubscribeOnParticleReset(ParticleUpdateDelegate update)
		{
			_onParticleReset -= update;
		}

		public void UpdateDefaults(Particle particle)
		{
			defaults = particle;
		}

	}
}
