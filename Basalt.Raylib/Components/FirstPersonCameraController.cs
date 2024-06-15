using Basalt.Common.Entities;
using Basalt.Math;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace Basalt.Raylib.Components
{
	public class FirstPersonCameraController : RayCameraController
	{
		public float Sensitivity = 0.1f;
		public FirstPersonCameraController(Entity entity) : base(entity)
		{
		}
		public override void OnUpdate()
		{
			if (!Enabled)
				return;

			Vector3 rotation = new(GetMouseDelta().X * Sensitivity,                            // Rotation: yaw
								   GetMouseDelta().Y * Sensitivity,                            // Rotation: pitch
								   0.0f);													   // Rotation: roll

			// Update the camera in raylib

			camera.Position = Entity.Transform.Position;
			camera.Target = camera.Position + Entity.Transform.Forward;

			UpdateCameraPro(ref camera, Vector3.Zero, rotation, 0);

			Entity.Transform.Rotation = BasaltMath.LookAtRotation(camera.Position, camera.Target, camera.Up);
		}
	}
}
