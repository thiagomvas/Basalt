using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Common.Physics;
using Raylib_cs;

namespace Basalt.Raylib.Components
{
	public class SphereRenderer : Component
	{
		public float Radius { get; set; } = 1.0f;
		public int Rings { get; set; } = 16;
		public int Slices { get; set; } = 16;

		public Color Color = Color.Pink;

		PhysicsEngine engine;
		
		public SphereRenderer(Entity entity) : base(entity)
		{

		}

		public override void OnStart()
		{
		}

		public override void OnUpdate()
		{
		}

		public override void OnRender()
		{
			Raylib_cs.Raylib.DrawSphereEx(entity.Transform.Position, Radius, Rings, Slices, Color);
		}
	}
}
