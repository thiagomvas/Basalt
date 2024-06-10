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
		/// Gets the Label displayed on the button.
		/// </summary>
		public Label Label { get; private set; }

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
		/// Gets or sets the action to be performed when the button is clicked.
		/// </summary>
		public Action OnClick { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Button"/> class.
		/// </summary>
		/// <param name="entity">The entity that the button belongs to.</param>
		/// <param name="label">The label containing text data for the button. It cannot be part of another entity or be instantiated.</param>
		public Button(Entity entity, Label label) : base(entity)
		{
			Label = label;
			label.Pivot = this.Pivot;
			label.Entity = this.Entity;
		}

		public override void OnStart()
		{
			Label.Pivot = this.Pivot;
			Label.Entity = this.Entity;
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

			Label.OnUIRender();
		}
	}
}