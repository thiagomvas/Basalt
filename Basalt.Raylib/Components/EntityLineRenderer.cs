using Basalt.Common.Components;
using Basalt.Common.Entities;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace Basalt.Raylib.Components
{
	public class EntityLineRenderer : Component
	{
		public Entity Target { get; set; }
		public float StartRadius { get; set; } = 1f;
		public float EndRadius { get; set; } = 1f;
		public int RenderSideCount { get; set; } = 16;
		public Color Color { get; set; }
		public Vector3 StartOffset { get; set; } = Vector3.Zero;
		public Vector3 EndOffset { get; set; } = Vector3.Zero;


		public EntityLineRenderer(Entity entity) : base(entity)
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
			if (Target is null)
			{
				return;
			}

			DrawCylinderEx(Entity.Transform.Position + StartOffset, Target.Transform.Position + EndOffset, StartRadius, EndRadius, RenderSideCount, Color);
		}
	}
}
