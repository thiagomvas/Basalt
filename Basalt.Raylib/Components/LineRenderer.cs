using Basalt.Common.Components;
using Basalt.Common.Entities;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace Basalt.Raylib.Components
{
	public class LineRenderer : Component
	{
		public float StartRadius { get; set; } = 1f;
		public float EndRadius { get; set; } = 1f;
		public bool RoundCorners { get; set; } = true;

		public int RenderSideCount { get; set; } = 16;
		public Color Color { get; set; }
		public Vector3[] Points { get; set; }

		public LineRenderer(Entity entity) : base(entity)
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
			if (Points.Length < 2)
			{
				return;
			}

			for (int i = 0; i < Points.Length - 1; i++)
			{
				float delta = (EndRadius - StartRadius) / (Points.Length - 1);
				float startRadius = StartRadius + delta * i;
				float endRadius = StartRadius + delta * (i + 1);
				DrawCylinderEx(Points[i], Points[i + 1], startRadius, endRadius, RenderSideCount, Color);
				if (RoundCorners) DrawSphere(Points[i + 1], endRadius, Color);
			}
		}
	}
}
