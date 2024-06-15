using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Core.Common.Attributes;
using Newtonsoft.Json;

namespace Basalt.Tests.Common
{
	[SingletonComponent]
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
		public bool HasStarted {get; set;}
		public int OnStartCount { get; set; } = 0;
		public int OnUpdateCount { get; set; } = 0; 
		public int OnRenderCount {get; set;} = 0; 
		public int OnPhysicsUpdateCount { get; set;} = 0;
		public int Foo { get; private set; } = 0;
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

		public int Get() => Foo;
		public void Set(int value) => Foo = value;

	}
}
