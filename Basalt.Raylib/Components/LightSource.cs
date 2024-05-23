using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Common.Utils;
using Basalt.Raylib.Graphics;
using Basalt.Raylib.Utils;
using Newtonsoft.Json;
using Raylib_cs;
using System.Numerics;

namespace Basalt.Raylib.Components
{
	/// <summary>
	/// Represents a light source component.
	/// </summary>
	public class LightSource : Component
	{
		[JsonIgnore]
		private static int _index = 0;

		[JsonIgnore]
		public Light Source;

		/// <summary>
		/// Gets or sets the color of the light source.
		/// </summary>
		public Color Color { get; set; } = Color.White;

		/// <summary>
		/// Gets or sets the type of the light source.
		/// </summary>
		public LightType Type { get; set; } = LightType.Point;

		private string shaderCacheKey;
		Shader shader;
		bool init = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="LightSource"/> class.
		/// </summary>
		/// <param name="entity">The entity that the light source belongs to.</param>
		/// <param name="shaderKey">The key of the shader used by the light source.</param>
		public LightSource(Entity entity, string shaderKey) : base(entity)
		{
			shaderCacheKey = shaderKey;
		}

		/// <summary>
		/// Called when the component starts.
		/// </summary>
		public override void OnStart()
		{
			RaylibGraphicsEngine.instantiateLight(this);
		}

		internal void Setup()
		{
			shader = ResourceCache.Instance.GetShader(shaderCacheKey)!.Value;
			Source = new Light();
			Source = Rlights.CreateLight(
				_index,
				Type,
				new Vector3(-2, 1, -2),
				Vector3.Zero,
				Color,
				shader);
			init = true;
		}

		/// <summary>
		/// Called every frame to update the component.
		/// </summary>
		public override void OnUpdate()
		{
			if (!init)
				return;
			Source.Position = Entity.Transform.Position;
		}

		/// <summary>
		/// Called when the component is destroyed.
		/// </summary>
		public override void OnDestroy()
		{
			RaylibGraphicsEngine.destroyLight(this);
		}
	}
}
