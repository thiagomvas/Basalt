using Basalt.Common.Components;
using Basalt.Common.Entities;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace Basalt.Raylib.Components
{
	/// <summary>
	/// Represents a line renderer component that renders lines and spheres in a 3D space.
	/// </summary>
	public class LineRenderer : Component
	{
		/// <summary>
		/// Gets or sets the start radius of the line.
		/// </summary>
		public float StartRadius { get; set; } = 1f;

		/// <summary>
		/// Gets or sets the end radius of the line.
		/// </summary>
		public float EndRadius { get; set; } = 1f;

		/// <summary>
		/// Gets or sets a value indicating whether the corners of the line should be rounded.
		/// </summary>
		public bool RoundCorners { get; set; } = true;

		/// <summary>
		/// Gets or sets the number of sides used to render the line.
		/// </summary>
		public int RenderSideCount { get; set; } = 16;

		/// <summary>
		/// Gets or sets the color of the line.
		/// </summary>
		public Color Color { get; set; }

		/// <summary>
		/// Gets or sets the points that define the line.
		/// </summary>
		public Vector3[] Points { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="LineRenderer"/> class.
		/// </summary>
		/// <param name="entity">The entity that owns this component.</param>
		public LineRenderer(Entity entity) : base(entity)
		{
		}

		/// <summary>
		/// Called when the component is started.
		/// </summary>
		public override void OnStart()
		{
		}

		/// <summary>
		/// Called every frame to update the component.
		/// </summary>
		public override void OnUpdate()
		{
		}

		/// <summary>
		/// Called every frame to render the component.
		/// </summary>
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
