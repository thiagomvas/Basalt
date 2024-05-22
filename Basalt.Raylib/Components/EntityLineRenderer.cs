using Basalt.Common.Components;
using Basalt.Common.Entities;
using Newtonsoft.Json;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace Basalt.Raylib.Components
{
	/// <summary>
	/// Represents a component that renders a line between two entities in a 3D space.
	/// </summary>
	public class EntityLineRenderer : Component
	{
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

		/// <summary>
		/// Gets or sets the start radius of the line.
		/// </summary>
		public float StartRadius { get; set; } = 1f;

		/// <summary>
		/// Gets or sets the end radius of the line.
		/// </summary>
		public float EndRadius { get; set; } = 1f;

		/// <summary>
		/// Gets or sets the number of sides used to render the line.
		/// </summary>
		public int RenderSideCount { get; set; } = 16;

		/// <summary>
		/// Gets or sets the color of the line.
		/// </summary>
		public Color Color { get; set; }

		/// <summary>
		/// Gets or sets the offset from the start entity's position.
		/// </summary>
		public Vector3 StartOffset { get; set; } = Vector3.Zero;

		/// <summary>
		/// Gets or sets the offset from the end entity's position.
		/// </summary>
		public Vector3 EndOffset { get; set; } = Vector3.Zero;

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityLineRenderer"/> class.
		/// </summary>
		/// <param name="entity">The entity that owns this component.</param>
		public EntityLineRenderer(Entity entity) : base(entity)
		{

		}

		/// <summary>
		/// Called when the component is started.
		/// </summary>
		public override void OnStart()
		{
			if (Target == null)
			{
				Target = Engine.Instance.EntityManager.GetEntities().Find(e => e.Id == TargetId);
			}
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
			if (Target is null)
			{
				return;
			}

			DrawCylinderEx(Entity.Transform.Position + StartOffset, Target.Transform.Position + EndOffset, StartRadius, EndRadius, RenderSideCount, Color);
		}

		public void SetRadius(float start, float end)
		{
			StartRadius = start;
			EndRadius = end;
		}
		public void SetRadius(float radius) => SetRadius(radius, radius);
	}
}
