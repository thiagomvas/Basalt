using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Raylib.Components;
using System.Drawing;
using System.Numerics;

namespace Basalt.TestField.Components
{
	internal class TestTrigger : Component
	{
		public TestTrigger(Entity entity) : base(entity)
		{
		}

		public override void OnStart()
		{
			Entity.AddComponent(new RaylibParticleSystem(Entity) { Looping = true, ModelCacheKey = "cube" });
			var ps = Entity.GetComponent<RaylibParticleSystem>();
			ps.UpdateDefaults(new() { Color = Color.RebeccaPurple, Velocity = new Vector3(0, 1f, 0) });
			ps.OnStartEvent(this, EventArgs.Empty);
		}

		public override void OnCollision(Collider other)
		{
			if (other.Entity.Id == "entity.player")
			{

			}
		}
	}
}
