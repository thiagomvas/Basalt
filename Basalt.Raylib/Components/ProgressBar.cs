using Basalt.Common.Components;
using Basalt.Common.Entities;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace Basalt.Raylib.Components
{
	public class ProgressBar : UIComponent
	{
		public Vector2 Size { get; set; } = Vector2.One;
		public Color BackgroundColor { get; set; } = Color.White;
		public Color ForegroundColor { get; set; } = Color.Green;
		public float Progress { get; set; } = 0.5f;
		public ProgressBar(Entity entity) : base(entity)
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
			DrawRectanglePro(new Rectangle(position.X, position.Y, Size.X, Size.Y), Size / 2, Rotation, BackgroundColor);
			DrawRectanglePro(new Rectangle(position.X, position.Y, Size.X * Progress, Size.Y), Size / 2, Rotation, ForegroundColor);
		}
	}
}
