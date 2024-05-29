using Basalt.Common.Components;
using Basalt.Common.Entities;
using Raylib_cs;

namespace Basalt.TestField.Components
{
	internal class TestUiComponent : UIComponent
	{
		public TestUiComponent(Entity entity) : base(entity)
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
			Raylib_cs.Raylib.DrawText("Hello, world!", 10, 10, 20, Color.White);
		}
	}
}
