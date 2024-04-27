using Basalt.Common;
using Basalt.Common.Components;
using Basalt.Common.Entities;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Basalt.TestField.Components
{
	public class HandController : Component
	{
		public Entity Player { get; set; }
		public Entity Shoulder { get; set; }

		private Entity? grabbed;
		private Collider? otherCol;
		float releasedTimer = 0;
		bool canGrab = true;
		public HandController(Entity entity) : base(entity)
		{
		}

		public override void OnStart()
		{
			Engine.Instance.InputSystem?.RegisterKeybind(
				new(Core.Common.Types.InputKey.F, Core.Common.Abstractions.Input.ActionType.Press), 
				Grab);
		}
		private void Grab()
		{
			Entity.Rigidbody.Velocity = Vector3.Zero;
			Entity.Transform.Position = Shoulder.Transform.Position + Player.Transform.Forward;
			Entity.Rigidbody.Velocity += Player.Transform.Forward * 25;
			if(grabbed != null)
			{
				grabbed.Transform.Position = Shoulder.Transform.Position + Player.Transform.Forward * 2;
				grabbed.Rigidbody.Velocity += Player.Transform.Forward * 25;
				grabbed.Rigidbody.IsKinematic = false;
				otherCol.Enabled = true;
				grabbed = null;
			}
		}
		public override void OnUpdate()
		{
			if (grabbed != null)
			{
				grabbed.Transform.Position = Entity.Transform.Position;
			}
			else
			{
				if (releasedTimer >= 0.25f)
					canGrab = true;
				releasedTimer += Time.DeltaTime;
			}
		}

		public override void OnCollision(Collider other)
		{
			if(other.Entity.Id.StartsWith("prop") && canGrab)
			{
				releasedTimer = 0;
				grabbed = other.Entity;
				otherCol = other;
				otherCol.Enabled = false;
				grabbed.Rigidbody.IsKinematic = true;
				grabbed.IsActive = false;
				canGrab = false;
			}
		}
	}
}
