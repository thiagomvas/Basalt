using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Math;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace Basalt.Raylib.Components
{
	/// <summary>
	/// Represents a button UI component.
	/// </summary>
	public class Button : UIComponent
	{
		/// <summary>
		/// Gets or sets the text displayed on the button.
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets the font size of the button text.
		/// </summary>
		public float FontSize { get; set; } = 12f;

		/// <summary>
		/// Gets or sets the spacing between characters in the button text.
		/// </summary>
		public float Spacing { get; set; } = 1f;

		/// <summary>
		/// Gets or sets the size of the button.
		/// </summary>
		public Vector2 Size { get; set; } = Vector2.One;

		/// <summary>
		/// Gets or sets the background color of the button.
		/// </summary>
		public Color BackgroundColor { get; set; } = Color.White;

		/// <summary>
		/// Gets or sets the color of the button when hovered over.
		/// </summary>
		public Color OnHoverColor { get; set; } = Color.Gray;

		/// <summary>
		/// Gets or sets the color of the button when clicked.
		/// </summary>
		public Color OnClickColor { get; set; } = Color.DarkGray;

		/// <summary>
		/// Gets or sets the color of the button text.
		/// </summary>
		public Color TextColor { get; set; } = Color.Black;

		/// <summary>
		/// Gets or sets the action to be performed when the button is clicked.
		/// </summary>
		public Action OnClick { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Button"/> class.
		/// </summary>
		/// <param name="entity">The entity that the button belongs to.</param>
		public Button(Entity entity) : base(entity)
		{
		}


		/// <inheritdoc/>
		public override void OnUIRender()
		{
			var position = GetPivotedPosition(new(GetScreenWidth(), GetScreenHeight())) + Offset;
			var mousePos = GetMousePosition();
			var rect = new Rectangle(position.X, position.Y, Size.X, Size.Y);

			if (BasaltMath.IsBetween(mousePos.X, position.X - Size.X * 0.5f, position.X + Size.X * 0.5f) && BasaltMath.IsBetween(mousePos.Y, position.Y - Size.Y * 0.5f, position.Y + Size.Y * 0.5f))
			{
				if (IsMouseButtonPressed(MouseButton.Left))
					OnClick?.Invoke();

				if (IsMouseButtonDown(MouseButton.Left))
					DrawRectanglePro(rect, Size / 2, Rotation, OnClickColor);
				else
					DrawRectanglePro(rect, Size / 2, Rotation, OnHoverColor);
			}
			else
			{
				DrawRectanglePro(rect, Size / 2, Rotation, BackgroundColor);
			}

			DrawTextPro(GetFontDefault(),
				Text,
				position,
				MeasureTextEx(GetFontDefault(), Text, FontSize, Spacing) / 2,
				Rotation,
				FontSize,
				Spacing,
				Color.White);
		}
	}
}
