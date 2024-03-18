using Basalt.Common.Entities;
using Basalt.Core.Common;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

namespace Basalt.Common.Components
{
	public sealed class Transform : Component
	{
		private Vector3 position;
		public Vector3 Position
		{
			get => position;
			set
			{
				var offset = value - position;
				position = value;
				foreach(var child in entity.Children)
				{
					child.Transform.Position += offset;
				}
			}
		}

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
