using Basalt.Common.Components;
using Basalt.Common.Entities;
using Raylib_cs;
using static Raylib_cs.Raylib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Basalt.Raylib.Graphics;
using Basalt.Common.Utils;
using Basalt.Common.Exceptions;

namespace Basalt.Raylib.Components
{
	public class ModelRenderer : Component
	{
		public Vector3 Size { get; set; } = Vector3.One;
		public Vector3 Offset { get; set; }
		public Color ColorTint { get; set; } = Color.White;
		public string ModelCacheKey { get; set; }
		public string LightingShaderCacheKey { get; set; }
		public float Scale = 1;
		Model cube;
		bool init = false;
		public ModelRenderer(Entity entity) : base(entity)
		{
		}

		public override void OnStart()
		{

		}

		public override void OnUpdate()
		{
		}

		public override unsafe void OnRender()
		{
			if (!Engine.Instance.Running)
				return;
			if (!init)
			{
				if (RaylibCache.Instance.HasModelKey(ModelCacheKey))
					cube = RaylibCache.Instance.GetModel(ModelCacheKey)!.Value;
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
