using Basalt.Common.Components;
using Basalt.Common.Entities;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace Basalt.Raylib.Components
{
	/// <summary>
	/// Represents a camera controller component that allows controlling a camera in a 3D scene.
	/// </summary>
	public class CameraController : Component
	{
		internal Camera3D camera;
		private float sensitivity = 0.1f;

		public CameraController(Entity entity) : base(entity)
		{
		}

		/// <summary>
		/// Called when the component is started.
		/// </summary>
		public override void OnStart()
		{
			// Initialize the camera
			camera = new Camera3D();
			camera.Position = Entity.Transform.Position;
			camera.Target = Entity.Transform.Position + Entity.Transform.Forward;
			camera.Up = new Vector3(0f, 1f, 0f);
			camera.FovY = 60f;
			camera.Projection = CameraProjection.Perspective;

			// Set the camera as the active camera in raylib
			UpdateCamera(ref camera, CameraMode.FirstPerson);
		}

		/// <summary>
		/// Called every frame to update the component.
		/// </summary>
		public override void OnUpdate()
		{
			if (!Enabled)
				return;

			Vector3 rotation = new(GetMouseDelta().X * sensitivity,                            // Rotation: yaw
								   GetMouseDelta().Y * sensitivity,                            // Rotation: pitch
								   0.0f);                                             // Rotation: roll

			// Update the camera in raylib

			camera.Position = Entity.Transform.Position;
			camera.Target = camera.Position + Entity.Transform.Forward;

			UpdateCameraPro(ref camera, Vector3.Zero, rotation, 0);

			Entity.Transform.Rotation = LookAtRotation(camera.Position, camera.Target, camera.Up);
		}

		private Quaternion LookAtRotation(Vector3 origin, Vector3 target, Vector3 up)
		{
			Vector3 forward = Vector3.Normalize(target - origin);
			Vector3 right = Vector3.Normalize(Vector3.Cross(up, forward));
			Vector3 newUp = Vector3.Cross(forward, right);

			Matrix4x4 matrix = new Matrix4x4(
				right.X, right.Y, right.Z, 0,
				newUp.X, newUp.Y, newUp.Z, 0,
				forward.X, forward.Y, forward.Z, 0,
				0, 0, 0, 1
			);

			return Quaternion.CreateFromRotationMatrix(matrix);
		}
	}
}
