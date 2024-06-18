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
				Entity.Destroy();
			}
		}
	}
}
