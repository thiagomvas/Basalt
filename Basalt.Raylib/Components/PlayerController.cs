using Basalt.Common;
using Basalt.Common.Components;
using Basalt.Common.Entities;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Basalt.Raylib.Components
{
	public class PlayerController : Component
	{
		public KeyboardKey Forward = KeyboardKey.Up, 
			Backward = KeyboardKey.Down, 
			Left = KeyboardKey.Left, 
			Right = KeyboardKey.Right;
		public float MoveSpeed = 5;
		public PlayerController(Entity entity) : base(entity)
		{
		}

		public override void OnStart()
		{
		}

		public override void OnUpdate()
		{

			if (!Entity.IsActive)
				return;
			if (IsKeyDown(Forward))
				Entity.Transform.Position += Entity.Transform.Forward * Time.DeltaTime * MoveSpeed;

			if(IsKeyDown(Backward))
				Entity.Transform.Position -= Entity.Transform.Forward * Time.DeltaTime * MoveSpeed;

			if(IsKeyDown(Left))
				Entity.Transform.Position -= Entity.Transform.Right * Time.DeltaTime * MoveSpeed;

			if(IsKeyDown(Right))
				Entity.Transform.Position += Entity.Transform.Right * Time.DeltaTime * MoveSpeed;
		}

	}
}
