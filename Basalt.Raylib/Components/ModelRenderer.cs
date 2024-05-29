using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Common.Exceptions;
using Basalt.Common.Utils;
using Basalt.Raylib.Graphics;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace Basalt.Raylib.Components
{
	/// <summary>
	/// Represents a component that renders a model.
	/// </summary>
	public class ModelRenderer : Component
	{
		/// <summary>
		/// Gets or sets the size of the model.
		/// </summary>
		public Vector3 Size { get; set; } = Vector3.One;

		/// <summary>
		/// Gets or sets the offset of the model.
		/// </summary>
		public Vector3 Offset { get; set; }

		/// <summary>
		/// Gets or sets the color tint of the model.
		/// </summary>
		public Color ColorTint { get; set; } = Color.White;

		/// <summary>
		/// Gets or sets the cache key for the model.
		/// </summary>
		public string ModelCacheKey { get; set; }

		/// <summary>
		/// Gets or sets the cache key for the lighting shader.
		/// </summary>
		public string LightingShaderCacheKey { get; set; }

		/// <summary>
		/// Gets or sets the scale of the model.
		/// </summary>
		public float Scale { get; set; } = 1;

		private Model cube;
		private bool init = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="ModelRenderer"/> class.
		/// </summary>
		/// <param name="entity">The entity that the component is attached to.</param>
		public ModelRenderer(Entity entity) : base(entity)
		{
		}


		/// <inheritdoc/>
		public override unsafe void OnRender()
		{
			if (!Engine.Instance.Running)
				return;

			if (!init)
			{
				if (ResourceCache.TryGetResource(ModelCacheKey, out cube))
					cube = ResourceCache.Instance.GetModel(ModelCacheKey)!.Value;
				else
				{
					throw new InvalidResourceKeyException(nameof(ModelCacheKey));
				}
				init = true;
			}

			cube.Transform = Raymath.MatrixRotateXYZ(Raymath.QuaternionToEuler(Entity.Transform.Rotation));
			DrawModelEx(cube, Entity.Transform.Position + Offset, Entity.Transform.Up, 0, Size, ColorTint);

		}
	}
}
