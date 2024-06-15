using Basalt.Common.Entities;
using Basalt.Math;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace Basalt.Raylib.Components
{
	/// <summary>
	/// Represents a first-person camera controller for Raylib.
	/// </summary>
	public class FirstPersonCameraController : RayCameraController
	{
		/// <summary>
		/// The sensitivity of the camera controller.
		/// </summary>
		public float Sensitivity = 0.1f;

		/// <summary>
		/// Initializes a new instance of the <see cref="FirstPersonCameraController"/> class.
		/// </summary>
		/// <param name="entity">The entity associated with the camera controller.</param>
		public FirstPersonCameraController(Entity entity) : base(entity)
		{
		}

		public override void OnUpdate()
		{
			if (!Enabled)
				return;

			Vector3 rotation = new(GetMouseDelta().X * Sensitivity,                            // Rotation: yaw
								   GetMouseDelta().Y * Sensitivity,                            // Rotation: pitch
								   0.0f);                                                       // Rotation: roll

			// Update the camera in raylib

			camera.Position = Entity.Transform.Position;
			camera.Target = camera.Position + Entity.Transform.Forward;

			UpdateCameraPro(ref camera, Vector3.Zero, rotation, 0);

			Entity.Transform.Rotation = BasaltMath.LookAtRotation(camera.Position, camera.Target, camera.Up);
		}
	}
}
