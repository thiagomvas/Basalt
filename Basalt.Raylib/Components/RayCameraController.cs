using Basalt.Common.Components;
using Basalt.Common.Entities;
using Raylib_cs;
using System.Numerics;

namespace Basalt.Raylib.Components
{
	public class RayCameraController : CameraControllerBase<Camera3D>
	{
		public RayCameraController(Entity entity) : base(entity)
		{
			Camera = new Camera3D
			{
				Position = Entity.Transform.Position,
				Target = Entity.Transform.Position + Entity.Transform.Forward,
				Up = new Vector3(0f, 1f, 0f),
				FovY = 60f,
				Projection = CameraProjection.Perspective
			};

			// Set the camera as the active camera in raylib
			Raylib_cs.Raylib.UpdateCamera(ref camera, CameraMode.FirstPerson);
		}
		protected Camera3D camera;
		public override Camera3D Camera { get => camera; set => camera = value; }
	}
}
