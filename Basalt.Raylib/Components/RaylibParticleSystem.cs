using Basalt.Common.Components;
using Basalt.Common.Entities;
using Raylib_cs;

namespace Basalt.Raylib.Components
{
	public class RaylibParticleSystem : BaseParticleSystem
	{
		public RaylibParticleSystem(Entity entity) : base(entity)
		{
		}

		public override void RenderParticles()
		{
			foreach(var particle in _particles)
			{
				Raylib_cs.Raylib.DrawSphere(particle.Position, 1f, Color.Red);
			}
		}
	}
}
