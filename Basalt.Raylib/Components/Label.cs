using Basalt.Common.Components;
using Basalt.Common.Entities;
using System.Numerics;

namespace Basalt.Raylib.Components
{
	public class Label : UIComponent
	{
		public string Text { get; set; } = string.Empty;
		public float FontSize { get; set; } = 20;
		public float Spacing { get; set; } = 1;
		public float Rotation { get; set; } = 0;
		public Label(Entity entity) : base(entity)
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
			var position = GetPivotedPosition(new(Raylib_cs.Raylib.GetScreenWidth(), Raylib_cs.Raylib.GetScreenHeight())) + Offset;
			var origin = new Vector2(0, 0);
			Raylib_cs.Raylib.DrawTextPro(Raylib_cs.Raylib.GetFontDefault(),
								Text,
								position,
								Raylib_cs.Raylib.MeasureTextEx(Raylib_cs.Raylib.GetFontDefault(), Text, FontSize, Spacing) / 2,
								Rotation,
								FontSize,
								Spacing,
								Raylib_cs.Color.White);
			Engine.Instance.Logger?.LogDebug($"Drawing label at {position}");
		}
	}
}
