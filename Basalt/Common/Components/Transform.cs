using Basalt.Common.Entities;
using Basalt.Core.Common;
using System.Numerics;

namespace Basalt.Common.Components
{
	public sealed class Transform : Component
	{
		public Vector3 Position { get; set; }
		public Quaternion Rotation { get; set; }
		internal Transform(Entity entity) : base(entity)
		{
			Position = new Vector3();

			Engine.Instance.EventBus?.Subscribe(this);
		}

		public override void OnStart()
		{

		}

		public override void OnUpdate()
		{

		}

		public override void OnPhysicsUpdate()
		{

		}
	}
}
