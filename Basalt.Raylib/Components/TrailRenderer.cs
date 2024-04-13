using Basalt.Common;
using Basalt.Common.Components;
using Basalt.Common.Entities;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace Basalt.Raylib.Components
{
	public class TrailRenderer : Component
	{
		public float StartRadius { get; set; } = 1f;
		public float EndRadius { get; set; } = 1f;
		public bool RoundCorners { get; set; } = true;
		public Vector3 Offset { get; set; } = Vector3.Zero;
		public int TrailSegmentCount { get; set; } = 16;
		public int RenderSideCount { get; set; } = 16;
		public Color Color { get; set; }
		public float TrailRefreshRate { get; set; } = 0.1f;


		float elapsedSinceLastPoint = 0;
		Vector3[] previousPosition = [];
		public TrailRenderer(Entity entity) : base(entity)
		{
		}

		public override void OnStart()
		{
		}

		public override void OnUpdate()
		{
			elapsedSinceLastPoint += Time.DeltaTime;
			if (elapsedSinceLastPoint > TrailRefreshRate)
			{
				elapsedSinceLastPoint -= TrailRefreshRate;
				Vector3[] newPreviousPosition = new Vector3[TrailSegmentCount];
				newPreviousPosition[0] = Entity.Transform.Position + Offset;
				for (int i = 1; i < previousPosition.Length; i++)
				{
					newPreviousPosition[i] = previousPosition[i - 1];
				}
				previousPosition = newPreviousPosition;
			}
		}

		public override void OnRender()
		{
			if (previousPosition.Length < 2)
			{
				return;
			}

			for (int i = 0; i < previousPosition.Length - 1; i++)
			{
				float delta = (EndRadius - StartRadius) / (previousPosition.Length - 1);
				float startRadius = StartRadius + delta * i;
				float endRadius = StartRadius + delta * (i + 1);
				DrawCylinderEx(previousPosition[i], previousPosition[i + 1], startRadius, endRadius, RenderSideCount, Color);
				if (RoundCorners) DrawSphere(previousPosition[i + 1], endRadius, Color);
			}

		}	
	}
}
