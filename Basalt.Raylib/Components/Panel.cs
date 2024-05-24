using Basalt.Common.Components;
using Basalt.Common.Entities;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;
namespace Basalt.Raylib.Components
{
	public class Panel : UIComponent
	{
		public Vector2 Size { get; set; } = Vector2.One;
		public Color Color { get; set; } = Color.White;
		public Panel(Entity entity) : base(entity)
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
			DrawRectanglePro(new Rectangle(position.X - Size.X / 2, position.Y - Size.Y / 2, Size.X, Size.Y), Size / 2, Rotation, Color);
		}
	}
}
