using Basalt.Common.Components;
using Basalt.Common.Entities;
using Newtonsoft.Json;

namespace Basalt.Tests.Common
{
	internal class TestComponent : Component
	{
		public TestComponent(Entity entity) : base(entity)
		{
		}

		[JsonProperty("TargetId")]
		public string TargetId { get; set; }
		[JsonIgnore]
		private Entity? target;
		[JsonIgnore]
		public Entity? Target
		{
			get => target;
			set
			{
				target = value;
				if (target != null)
				{
					TargetId = target.Id;
				}
			}
		}
		public bool HasStarted;
		public int OnStartCount = 0, OnUpdateCount = 0, OnRenderCount = 0, OnPhysicsUpdateCount = 0;
		public override void OnStart()
		{
			HasStarted = true;
			OnStartCount++;

			if (Target == null && !string.IsNullOrEmpty(TargetId))
			{
				Target = Engine.Instance.EntityManager.GetEntities().Find(e => e.Id == TargetId);
			}
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
