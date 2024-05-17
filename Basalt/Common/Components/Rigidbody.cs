using Basalt.Common.Entities;
using Basalt.Common.Physics;
using Basalt.Core.Common.Abstractions.Engine;
using Basalt.Core.Common.Attributes;
using System.Numerics;

namespace Basalt.Common.Components
{
	/// <summary>
	/// Represents a rigidbody component that simulates physics behavior.
	/// </summary>
	[SingletonComponent]
	public class Rigidbody : Component
	{
		/// <summary>
		/// The mass of the rigidbody.
		/// </summary>
		public float Mass = 1;

		/// <summary>
		/// The drag coefficient of the rigidbody.
		/// </summary>
		public float Drag = 0.1f;

		/// <summary>
		/// Determines if the rigidbody is kinematic (immovable).
		/// </summary>
		public bool IsKinematic = false;

		/// <summary>
		/// The velocity of the rigidbody.
		/// </summary>
		public Vector3 Velocity;
		
		private const float threshold = 0.0001f;

		/// <summary>
		/// Initializes a new instance of the <see cref="Rigidbody"/> class.
		/// </summary>
		/// <param name="entity">The entity that the rigidbody belongs to.</param>
		public Rigidbody(Entity entity) : base(entity)
		{
			var physics = Engine.Instance.GetEngineComponent<IPhysicsEngine>();
			if (physics != null)
			{
				physicsEngine = physics;
				if(physics is PhysicsEngine engine)
					chunking = engine.chunking;
			}
			else
			{
				Engine.Instance.Logger?.LogError("Could not find a physics engine component that implements IPhysicsEngine. Changed rigidbody to be kinematic");
				IsKinematic = true;
			}
		}

		private IPhysicsEngine physicsEngine;
		private IChunkingMechanism? chunking;

		/// <summary>
		/// Called on each physics update frame.
		/// </summary>
		public override void OnPhysicsUpdate()
		{
			if (IsKinematic)
			{
				return;
			}

			var lengthSqr = Velocity.LengthSquared();

			if(lengthSqr < threshold || lengthSqr == float.NaN)
			{
				Velocity = Vector3.Zero;
			}

			Vector3? acceleration = -Vector3.UnitY * physicsEngine.Gravity;

			if (acceleration.HasValue)
			{
				Velocity += acceleration.Value * Time.PhysicsDeltaTime;
			}

			Vector3 prevPos = Entity.Transform.Position;
			Entity.Transform.Position += Velocity * Time.PhysicsDeltaTime;

			if(lengthSqr > threshold)
			{
				Velocity -= Velocity * Drag * Time.PhysicsDeltaTime;
				chunking?.MarkForUpdate(Entity);
			}

		}

		/// <summary>
		/// Called when the component starts.
		/// </summary>
		public override void OnStart()
		{

		}

		/// <summary>
		/// Called on each frame update.
		/// </summary>
		public override void OnUpdate()
		{
		}

		public void AddForce(Vector3 force, ForceType type = ForceType.Force)
		{
			if (IsKinematic)
			{
				return;
			}

			switch (type)
			{
				case ForceType.Force:
					Velocity += force / Mass;
					break;
				case ForceType.Impulse:
					Velocity += force;
					break;
			}

		}
	}
}
