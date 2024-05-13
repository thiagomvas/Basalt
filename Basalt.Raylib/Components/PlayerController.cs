using Basalt.Common;
using Basalt.Common.Components;
using Basalt.Common.Entities;
using Raylib_cs;
using System.Numerics;
using Basalt.Common.Utils.Extensions;
using static Raylib_cs.Raylib;

namespace Basalt.Raylib.Components
{
	public class PlayerController : Component
	{
		/// <summary>
		/// The key used to move the player forward.
		/// </summary>
		public KeyboardKey Forward = KeyboardKey.Up;

		/// <summary>
		/// The key used to move the player backward.
		/// </summary>
		public KeyboardKey Backward = KeyboardKey.Down;

		/// <summary>
		/// The key used to move the player left.
		/// </summary>
		public KeyboardKey Left = KeyboardKey.Left;

		/// <summary>
		/// The key used to move the player right.
		/// </summary>
		public KeyboardKey Right = KeyboardKey.Right;

		/// <summary>
		/// The key used to make the player jump.
		/// </summary>
		public KeyboardKey Jump = KeyboardKey.Space;

		/// <summary>
		/// The speed at which the player moves.
		/// </summary>
		public float MoveSpeed = 5;

		/// <summary>
		/// Initializes a new instance of the <see cref="PlayerController"/> class.
		/// </summary>
		/// <param name="entity">The entity that this component is attached to.</param>
		public PlayerController(Entity entity) : base(entity)
		{

		}

		/// <inheritdoc/>
		public override void OnStart()
		{
		}

		/// <inheritdoc/>
		public override void OnUpdate()
		{
			if (!Entity.Enabled)
				return;

			if (IsKeyDown(Forward))
				Entity.Transform.Position += Entity.Transform.Forward.XZNormalized() * Time.DeltaTime * MoveSpeed;

			if (IsKeyDown(Backward))
				Entity.Transform.Position -= Entity.Transform.Forward.XZNormalized() * Time.DeltaTime * MoveSpeed;

			if (IsKeyDown(Left))
				Entity.Transform.Position -= Entity.Transform.Right.XZNormalized() * Time.DeltaTime * MoveSpeed;

			if (IsKeyDown(Right))
				Entity.Transform.Position += Entity.Transform.Right.XZNormalized() * Time.DeltaTime * MoveSpeed;

			if (IsKeyPressed(Jump) && Entity.Rigidbody is not null)
				Entity.Rigidbody!.Velocity += new Vector3(0, 1, 0) * MoveSpeed;
		}
	}
}
