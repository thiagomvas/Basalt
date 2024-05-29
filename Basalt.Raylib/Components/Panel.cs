using Basalt.Common.Components;
using Basalt.Common.Entities;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;
namespace Basalt.Raylib.Components
{
	/// <summary>
	/// Represents a panel UI component.
	/// </summary>
	public class Panel : UIComponent
	{
		/// <summary>
		/// Gets or sets the size of the panel.
		/// </summary>
		public Vector2 Size { get; set; } = Vector2.One;

		/// <summary>
		/// Gets or sets the color of the panel.
		/// </summary>
		public Color Color { get; set; } = Color.White;

		/// <summary>
		/// Initializes a new instance of the <see cref="Panel"/> class.
		/// </summary>
		/// <param name="entity">The entity that the panel belongs to.</param>
		public Panel(Entity entity) : base(entity)
		{
		}

		/// <inheritdoc/>
		public override void OnStart()
		{
		}

		/// <inheritdoc/>
		public override void OnUpdate()
		{
		}

		/// <inheritdoc/>
		public override void OnUIRender()
		{
			var position = GetPivotedPosition(new(GetScreenWidth(), GetScreenHeight())) + Offset;
			DrawRectanglePro(new Rectangle(position.X, position.Y, Size.X, Size.Y), Size / 2, Rotation, Color);
		}
	}
}
