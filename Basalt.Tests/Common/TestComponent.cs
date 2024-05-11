using Basalt.Common.Components;
using Basalt.Common.Entities;

namespace Basalt.Tests.Common
{
	internal class TestComponent : Component
	{
		public TestComponent(Entity entity) : base(entity)
		{
		}

		public int OnStartCount = 0, OnUpdateCount = 0, OnRenderCount = 0, OnPhysicsUpdateCount = 0;
		public override void OnStart()
		{
			OnStartCount++;
		}

		public override void OnUpdate()
		{
			OnUpdateCount++;
		}

		public override void OnRender()
		{
			OnRenderCount++;
		}

		public override void OnPhysicsUpdate()
		{
			OnPhysicsUpdateCount++;
		}

	}
}
