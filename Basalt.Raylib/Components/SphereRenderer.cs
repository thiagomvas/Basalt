using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Common.Utils;
using Basalt.Raylib.Graphics;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace Basalt.Raylib.Components
{
	/// <summary>
	/// Represents a component that renders a sphere.
	/// </summary>
	public class SphereRenderer : Component
	{
		private Vector3 size;

		/// <summary>
		/// Gets or sets the size of the sphere.
		/// </summary>
		public Vector3 Size
		{
			get { return size; }
			set { size = value / 2; }
		}

		/// <summary>
		/// Gets or sets the number of rings in the sphere.
		/// </summary>
		public int Rings { get; set; } = 16;

		/// <summary>
		/// Gets or sets the number of slices in the sphere.
		/// </summary>
		public int Slices { get; set; } = 16;

		/// <summary>
		/// Gets or sets the color of the sphere.
		/// </summary>
		public Color Color { get; set; } = Color.Pink;

		/// <summary>
		/// Gets or sets the offset of the sphere.
		/// </summary>
		public Vector3 Offset { get; set; } = Vector3.Zero;

		private Model sphere;
		private bool init;

		/// <summary>
		/// Initializes a new instance of the <see cref="SphereRenderer"/> class.
		/// </summary>
		/// <param name="entity">The entity that the component is attached to.</param>
		public SphereRenderer(Entity entity) : base(entity)
		{

		}

		/// <inheritdoc/>
		public override void OnStart()
		{
		}

		/// <inheritdoc/>
		public override void OnUpdate()
		{
		}

		/// <inheritdoc/>
		public override unsafe void OnRender()
		{
			if (!Engine.Instance.Running)
				return;
			if (!init)
			{
				if (!ResourceCache.TryGetResource("sphere", out sphere))
				{
					sphere = LoadModelFromMesh(GenMeshSphere(1, Rings, Slices));
					//if(ResourceCache.Instance.HasShaderKey("lighting"))
					//	sphere.Materials[0].Shader = ResourceCache.Instance.GetShader("lighting")!.Value;
					ResourceCache.CacheResource("sphere", sphere);
				}
				else
					sphere = ResourceCache.Instance.GetModel("sphere")!.Value;

				init = true;
			}
			DrawModelEx(sphere, Entity.Transform.Position + Offset, new Vector3(0, 0, 1), 0, Size, Color);

		}
	}
}
