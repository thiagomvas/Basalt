using Basalt.Common.Components;
using Basalt.Common.Entities;
using Raylib_cs;
using System.Numerics;

namespace Basalt.Raylib.Components
{
	/// <summary>
	/// Represents a label UI component.
	/// </summary>
	public class Label : UIComponent
	{
		/// <summary>
		/// Gets or sets the text of the label.
		/// </summary>
		public string Text { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the font size of the label.
		/// </summary>
		public float FontSize { get; set; } = 20;

		/// <summary>
		/// Gets or sets the spacing between characters in the label.
		/// </summary>
		public float Spacing { get; set; } = 1;

		/// <summary>
		/// Gets or sets the color of the text in the label.
		/// </summary>
		public Color TextColor { get; set; } = Color.White;

		/// <summary>
		/// Initializes a new instance of the <see cref="Label"/> class.
		/// </summary>
		/// <param name="entity">The entity that the label belongs to.</param>
		public Label(Entity entity) : base(entity)
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
			var position = GetPivotedPosition(new(Raylib_cs.Raylib.GetScreenWidth(), Raylib_cs.Raylib.GetScreenHeight())) + Offset;
			var origin = new Vector2(0, 0);
			Raylib_cs.Raylib.DrawTextPro(Raylib_cs.Raylib.GetFontDefault(),
								Text,
								position,
								Raylib_cs.Raylib.MeasureTextEx(Raylib_cs.Raylib.GetFontDefault(), Text, FontSize, Spacing) / 2,
								Rotation,
								FontSize,
								Spacing,
								TextColor);
		}
	}
}
