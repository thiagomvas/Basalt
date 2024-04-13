using Basalt.Common.Entities;
using Basalt.Common.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Basalt.Common.Components
{
	/// <summary>
	/// Represents a component that defines the position and rotation of an entity in 3D space.
	/// </summary>
	public sealed class Transform : Component
	{
		public bool IsFixedPoint { get; set; } = false;
		private Vector3 position;
		/// <summary>
		/// Gets or sets the position of the transform.
		/// </summary>
		public Vector3 Position
		{
			get => position;
			set
			{
				if(IsFixedPoint)
				{
					return;
				}

				var offset = value - position;
				position = value;
				UpdateChildTransforms(offset, Quaternion.Identity);
			}
		}

		private Quaternion rotation = Quaternion.Identity;
		/// <summary>
		/// Gets or sets the rotation of the transform.
		/// </summary>
		public Quaternion Rotation
		{
			get => rotation;
			set
			{
				Quaternion oldRotation = rotation;
				rotation = value;
				Quaternion rotationChange = value * Quaternion.Inverse(oldRotation);
				UpdateChildTransforms(Vector3.Zero, rotationChange);
			}
		}
		private void UpdateChildTransforms(Vector3 positionOffset, Quaternion rotationChange)
		{
			foreach (var child in Entity.Children)
			{
				// Apply rotation change to child's position offset from parent
				Vector3 offsetFromParent = child.Transform.Position - position;
				Vector3 newPositionOffsetRotated = Vector3.Transform(offsetFromParent, rotationChange);

				// Calculate the new position of the child
				Vector3 newPosition = position + newPositionOffsetRotated + positionOffset;
				child.Transform.Position = newPosition;

				// Apply rotation change to child's rotation
				child.Transform.Rotation *= rotationChange;
			}
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

		[JsonIgnore]
		public Vector3 Up => Vector3.Cross(Forward, Right);

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
			if(Position.X == float.NaN)
				Position = Vector3.Zero;
		}
	}
}
