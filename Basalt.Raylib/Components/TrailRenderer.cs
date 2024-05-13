using Basalt.Common;
using Basalt.Common.Components;
using Basalt.Common.Entities;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace Basalt.Raylib.Components
{
	/// <summary>
	/// Represents a trail renderer component that creates a trail effect behind an entity.
	/// </summary>
	public class TrailRenderer : Component
	{
		/// <summary>
		/// Gets or sets the starting radius of the trail.
		/// </summary>
		public float StartRadius { get; set; } = 1f;

		/// <summary>
		/// Gets or sets the ending radius of the trail.
		/// </summary>
		public float EndRadius { get; set; } = 1f;

		/// <summary>
		/// Gets or sets a value indicating whether the corners of the trail should be rounded.
		/// </summary>
		public bool RoundCorners { get; set; } = true;

		/// <summary>
		/// Gets or sets the offset of the trail from the entity's position.
		/// </summary>
		public Vector3 Offset { get; set; } = Vector3.Zero;

		/// <summary>
		/// Gets or sets the number of segments in the trail.
		/// </summary>
		public int TrailSegmentCount { get; set; } = 16;

		/// <summary>
		/// Gets or sets the number of sides used to render the trail.
		/// </summary>
		public int RenderSideCount { get; set; } = 16;

		/// <summary>
		/// Gets or sets the color of the trail.
		/// </summary>
		public Color Color { get; set; }

		/// <summary>
		/// Gets or sets the refresh rate of the trail in seconds.
		/// </summary>
		public float TrailRefreshRate { get; set; } = 0.1f;

		private float elapsedSinceLastPoint = 0;
		private Vector3[] previousPosition = new Vector3[0];

		/// <summary>
		/// Initializes a new instance of the <see cref="TrailRenderer"/> class.
		/// </summary>
		/// <param name="entity">The entity to attach the trail renderer to.</param>
		public TrailRenderer(Entity entity) : base(entity)
		{
		}

		/// <inheritdoc/>
		public override void OnStart()
		{
		}

		/// <inheritdoc/>
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

		/// <inheritdoc/>
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
