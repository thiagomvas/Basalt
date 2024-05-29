using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Math;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace Basalt.Raylib.Components
{
	public class Button : UIComponent
	{
		public string Text { get; set; }
		public float FontSize { get; set; } = 12f;
		public float Spacing { get; set; } = 1f;
		public Vector2 Size { get; set; } = Vector2.One;
		public Color BackgroundColor { get; set; } = Color.White;
		public Color OnHoverColor { get; set; } = Color.Gray;
		public Color OnClickColor { get; set; } = Color.DarkGray;
		public Color TextColor { get; set; } = Color.Black;

		public Action OnClick { get; set; }
		public Button(Entity entity) : base(entity)
		{
		}

		public override void OnStart()
		{

		}

		public override void OnUpdate()
		{

		}

		public override void OnUIRender()
		{

			var position = GetPivotedPosition(new(GetScreenWidth(), GetScreenHeight())) + Offset;
			var mousePos = GetMousePosition();
			var rect = new Rectangle(position.X, position.Y, Size.X, Size.Y);
			if (BasaltMath.IsBetween(mousePos.X, position.X - Size.X * 0.5f, position.X + Size.X * 0.5f) && BasaltMath.IsBetween(mousePos.Y, position.Y - Size.Y * 0.5f, position.Y + Size.Y * 0.5f))
			{
				if (IsMouseButtonPressed(MouseButton.Left))
				{
					OnClick?.Invoke();
				}
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
