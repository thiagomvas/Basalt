using Basalt.Common.Components;
using Basalt.Common.Entities;
using Raylib_cs;
using System;
using static Raylib_cs.Raylib;
using System.Numerics;
using Basalt.Common;

namespace Basalt.Raylib.Components
{
	public class CameraController : Entity
	{
		internal Camera3D camera;
		private float cameraSpeed = 5f;
		private float sensitivity = 0.2f;
		private float MovementSpeed = 5f;

		private Vector3 forward, right;
		public void OnStart()
		{
			// Initialize the camera
			camera = new Camera3D();
			camera.Position = Transform.Position;
			camera.Target = new Vector3(0f, 0f, -1f);
			camera.Up = new Vector3(0f, 1f, 0f);
			camera.FovY = 45f;
			camera.Projection = CameraProjection.Perspective;

			// Set the camera as the active camera in raylib
			UpdateCamera(ref camera, CameraMode.FirstPerson);

			forward = Vector3.UnitX * MovementSpeed;
			right = Vector3.UnitY * MovementSpeed;
		}

		public void OnUpdate()
		{
			Vector3 movement = Vector3.Zero;

			if (IsKeyDown(KeyboardKey.W))
				movement += forward * Time.DeltaTime;
			if(IsKeyDown(KeyboardKey.S))
				movement -= forward * Time.DeltaTime;
			if(IsKeyDown(KeyboardKey.A))
				movement -= right * Time.DeltaTime;
			if(IsKeyDown(KeyboardKey.D))
				movement += right * Time.DeltaTime;


			Vector3 rotation = new(GetMouseDelta().X * 0.05f,                            // Rotation: yaw
								   GetMouseDelta().Y * 0.05f,                            // Rotation: pitch
								   0.0f);                                             // Rotation: roll

			// Update the camera in raylib
			UpdateCameraPro(ref camera, movement, rotation, 0);

			Transform.Position = camera.Position;
		}
	}
}
