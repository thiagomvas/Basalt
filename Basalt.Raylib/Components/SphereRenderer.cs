using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Common.Physics;
using Raylib_cs;
using System.Numerics;

namespace Basalt.Raylib.Components
{
	public class SphereRenderer : Component
	{
		public float Radius { get; set; } = 1.0f;
		public int Rings { get; set; } = 16;
		public int Slices { get; set; } = 16;

		public Color Color = Color.Pink;

		public Vector3 Offset { get; set; } = Vector3.Zero;
		
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
			Raylib_cs.Raylib.DrawSphereEx(Entity.Transform.Position + Offset, Radius, Rings, Slices, Color);
		}
	}
}
