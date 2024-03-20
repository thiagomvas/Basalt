using Basalt.Common.Entities;
using Basalt.Common.Utils;
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
				foreach (var child in entity.Children)
				{
					child.Transform.Position += offset;
				}
			}
		}

		private Quaternion rotation;
		public Quaternion Rotation
		{
			get => rotation;
			set => rotation = value;
		}

		public Vector3 Forward => MathExtended.GetForwardVector(Rotation);
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
	}
}
