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
	public class LightSource : Component
	{
		[JsonIgnore]
		private static int _index = 0;

		[JsonIgnore]
		public Light Source;

		public Color Color { get; set; } = Color.White;
		public LightType Type { get; set; } = LightType.Point;
		public string ShaderCacheKey { get; set; }
		Shader shader;
		bool init = false;
		public LightSource(Entity entity, string shaderKey) : base(entity)
		{
			ShaderCacheKey = shaderKey;
		}
		public override void OnStart()
		{
			RaylibGraphicsEngine.instantiateLight(this);
		}
		internal void Setup()
		{
			shader = RaylibCache.Instance.GetShader(ShaderCacheKey)!.Value;
			Source = new Light(); 
			Source = Rlights.CreateLight(
				_index,
				LightType.Point,
				new Vector3(-2, 1, -2),
				Vector3.Zero,
				Color.Yellow,
				shader);
			init = true;
		}
		public override void OnUpdate()
		{
			if (!init)
				return;
			Source.Position = Entity.Transform.Position;
		}

		public override void OnDestroy()
		{
			RaylibGraphicsEngine.destroyLight(this);
		}

	}
}
