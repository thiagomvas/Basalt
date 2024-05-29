using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Common.Utils;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace Basalt.Raylib.Components
{
	/// <summary>
	/// Represents a component that renders a box shape.
	/// </summary>
	public class BoxRenderer : Component
	{
		/// <summary>
		/// Gets or sets the size of the box.
		/// </summary>
		public Vector3 Size { get; set; } = Vector3.One;

		/// <summary>
		/// Gets or sets the offset of the box.
		/// </summary>
		public Vector3 Offset { get; set; }

		/// <summary>
		/// Gets or sets the color of the box.
		/// </summary>
		public Color Color { get; set; } = Color.White;

		/// <summary>
		/// Gets or sets the scale of the box.
		/// </summary>
		public float Scale = 1;

		private Model cube;
		private bool init = false;

		/// <summary>
		/// Gets or sets a value indicating whether the rotation of the box is locked.
		/// </summary>
		public bool LockRotation { get; set; } = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="BoxRenderer"/> class.
		/// </summary>
		/// <param name="entity">The entity that the component is attached to.</param>
		public BoxRenderer(Entity entity) : base(entity)
		{
		}


		/// <summary>
		/// Called every frame to render the component.
		/// </summary>
		public override unsafe void OnRender()
		{
			if (!Engine.Instance.Running)
				return;
			if (!init)
			{
				if (!ResourceCache.TryGetResource("cube", out cube))
				{
					cube = LoadModelFromMesh(GenMeshCube(1, 1, 1));
					ResourceCache.CacheResource("cube", cube);
				}

				init = true;
			}
			cube.Transform = Raymath.MatrixRotateXYZ(Raymath.QuaternionToEuler(Entity.Transform.Rotation));
			DrawModelEx(cube, Entity.Transform.Position + Offset, Entity.Transform.Up, 0, Size, Color);

		}
	}
}
