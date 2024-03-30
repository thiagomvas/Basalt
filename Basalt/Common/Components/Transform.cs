using Basalt.Common.Entities;
using Basalt.Common.Utils;
using Newtonsoft.Json;
using System.Numerics;

namespace Basalt.Common.Components
{
	/// <summary>
	/// Represents a component that defines the position and rotation of an entity in 3D space.
	/// </summary>
	public sealed class Transform : Component
	{
		private Vector3 position;
		/// <summary>
		/// Gets or sets the position of the transform.
		/// </summary>
		public Vector3 Position
		{
			get => position;
			set
			{
				var offset = value - position;
				position = value;
				foreach (var child in Entity.Children)
				{
					child.Transform.Position += offset;
				}
			}
		}

		private Quaternion rotation = Quaternion.Identity;
		/// <summary>
		/// Gets or sets the rotation of the transform.
		/// </summary>
		public Quaternion Rotation
		{
			get => rotation;
			set => rotation = value;
		}

		/// <summary>
		/// Gets the forward vector of the transform based on its rotation.
		/// </summary>
		[JsonIgnore]
		public Vector3 Forward => MathExtended.GetForwardVector(Rotation);

		/// <summary>
		/// Gets the right vector of the transform based on its rotation.
		/// </summary>
		[JsonIgnore]
		public Vector3 Right => MathExtended.GetRightVector(Rotation);

		/// <summary>
		/// Initializes a new instance of the <see cref="Transform"/> class.
		/// </summary>
		/// <param name="entity">The entity that the transform belongs to.</param>
		public Transform(Entity entity) : base(entity)
		{
			Position = new Vector3();

			Engine.Instance.EventBus?.Subscribe(this);
		}

		/// <summary>
		/// Called when the component starts.
		/// </summary>
		public override void OnStart()
		{

		}

		/// <summary>
		/// Called every frame to update the component.
		/// </summary>
		public override void OnUpdate()
		{

		}
	}
}
