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

namespace Basalt.Raylib.Components
{
	public class BoxRenderer : Component
	{
		public Vector3 Size { get; set; } = Vector3.One;
		public Vector3 Offset { get; set; }
		public Color Color { get; set; } = Color.White;
		public float Scale = 1;
		Model cube;
		bool init = false;
		public BoxRenderer(Entity entity) : base(entity)
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
				if (RaylibCache.Instance.HasModelKey("cube"))
					cube = RaylibCache.Instance.GetModel("cube")!.Value;

				else
				{
					Model c = LoadModelFromMesh(GenMeshCube(1, 1, 1));
					RaylibCache.Instance.CacheModel("cube", c);
				}

				init = true;
			}
			cube.Transform = Raymath.MatrixRotateXYZ(Raymath.QuaternionToEuler(Entity.Transform.Rotation));
			DrawModelEx(cube, Entity.Transform.Position + Offset, Entity.Transform.Up, 0, Size, Color);
		}
	}
}
