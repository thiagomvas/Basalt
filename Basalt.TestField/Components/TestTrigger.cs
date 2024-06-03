using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Raylib.Components;

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
				Entity.GetComponent<ModelRenderer>().ModelCacheKey = "sphere";
			}
		}
	}
}
