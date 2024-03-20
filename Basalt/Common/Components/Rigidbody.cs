using Basalt.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Common.Components
{
	public class Rigidbody : Component
	{
		public float Mass = 1;
		public float Drag = 0.1f;
		public bool IsKinematic = false;

		public Vector3 Velocity;
		public Rigidbody(Entity entity) : base(entity)
		{
		}


		public override void OnPhysicsUpdate()
		{
			if(IsKinematic)
			{
				return;
			}
			Vector3? acceleration = -Vector3.UnitY * Engine.PhysicsEngine?.Gravity;

			if (acceleration.HasValue)
			{
				Velocity += acceleration.Value * Time.PhysicsDeltaTime;
			}



			Vector3 prevPos = Entity.Transform.Position;
			Entity.Transform.Position += Velocity * Time.PhysicsDeltaTime;

			Vector3 delta = Entity.Transform.Position - prevPos;

		}
		public override void OnStart()
		{
		}

		public override void OnUpdate()
		{
		}
	}
}
