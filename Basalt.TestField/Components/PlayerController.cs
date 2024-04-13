using Basalt.Common;
using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Common.Utils.Extensions;
using Basalt.Core.Common.Abstractions.Input;
using Basalt.Core.Common.Types;
using System.Numerics;

namespace Basalt.TestField.Components
{
	public class PlayerController : Component
	{
		public float MoveSpeed = 5;
		public PlayerController(Entity entity) : base(entity)
		{

		}

		public override void OnStart()
		{
			Engine.Instance.InputSystem?.RegisterKeybind(new(
				InputKey.W,
				ActionType.Hold),
				() => Entity.Transform.Position += Entity.Transform.Forward.XZNormalized() * Time.DeltaTime * MoveSpeed);

			Engine.Instance.InputSystem?.RegisterKeybind(new(
				InputKey.S,
				ActionType.Hold),
				() => Entity.Transform.Position -= Entity.Transform.Forward.XZNormalized() * Time.DeltaTime * MoveSpeed);

			Engine.Instance.InputSystem?.RegisterKeybind(new(
				InputKey.A,
				ActionType.Hold),
				() => Entity.Transform.Position -= Entity.Transform.Right.XZNormalized() * Time.DeltaTime * MoveSpeed);

			Engine.Instance.InputSystem?.RegisterKeybind(new(
				InputKey.D,
				ActionType.Hold),
				() => Entity.Transform.Position += Entity.Transform.Right.XZNormalized() * Time.DeltaTime * MoveSpeed);

			Engine.Instance.InputSystem?.RegisterKeybind(new(
				InputKey.Space,
				ActionType.Press),
				() => Entity.Rigidbody.Velocity += Vector3.UnitY * MoveSpeed);
		}

		public override void OnUpdate()
		{
			if (!Entity.IsActive)
				return;
		}
	}
}
