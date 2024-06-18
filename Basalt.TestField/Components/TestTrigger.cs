using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Raylib.Components;
using System.Numerics;

namespace Basalt.TestField.Components
{
	internal class TestTrigger : Component
	{
		public TestTrigger(Entity entity) : base(entity)
		{
		}

		public override void OnCollision(Collider other)
		{
			if (other.Entity.Id == "entity.player")
			{
				Entity.Collider!.Enabled = false;
				Console.WriteLine($"Trigger collided with {other.Entity.Id}");
				Entity.GetComponent<ModelRenderer>().ModelCacheKey = "sphere";
				Entity.AddComponent(new RaylibParticleSystem(Entity) { Looping = true, ModelCacheKey = "knot"});
				var ps = Entity.GetComponent<RaylibParticleSystem>();
				ps.UpdateDefaults(new Types.Particle { Velocity = Vector3.UnitY });
			}
		}
	}
}
