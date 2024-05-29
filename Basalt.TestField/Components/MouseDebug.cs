using Basalt.Common.Components;
using Basalt.Common.Entities;

namespace Basalt.TestField.Components
{
	public class MouseDebug : UIComponent
	{
		public MouseDebug(Entity entity) : base(entity)
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
			Raylib_cs.Raylib.DrawCircleV(Raylib_cs.Raylib.GetMousePosition(), 10, Raylib_cs.Color.Purple);
		}
	}
}
