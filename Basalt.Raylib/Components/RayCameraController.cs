using Basalt.Common.Components;
using Basalt.Common.Entities;
using Raylib_cs;
using System.Numerics;

namespace Basalt.Raylib.Components
{
	/// <summary>
	/// Represents a camera controller for Raylib using Camera3D.
	/// </summary>
	public class RayCameraController : CameraControllerBase<Camera3D>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RayCameraController"/> class.
		/// </summary>
		/// <param name="entity">The entity associated with the camera controller.</param>
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

		/// <summary>
		/// Gets or sets the camera.
		/// </summary>
		public override Camera3D Camera { get => camera; set => camera = value; }
	}
}
