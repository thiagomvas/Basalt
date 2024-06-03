using Basalt.Common.Components;
using Basalt.Common.Entities;

namespace Basalt.TestField.Components
{
	internal class TestTrigger : Component
	{
		public TestTrigger(Entity entity) : base(entity)
		{
		}

		public override void OnCollision(Collider other)
		{
			if(other.Entity.Id == "entity.player")
			{
				Console.WriteLine($"Trigger collided with {other.Entity.Id}");
				Entity.Destroy();
			}
		}

		public override void OnDestroy()
		{
			Console.WriteLine("Destroy");
		}
	}
}
