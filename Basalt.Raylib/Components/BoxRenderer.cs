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

namespace Basalt.Raylib.Components
{
	public class BoxRenderer : Component
	{
		public Vector3 Size { get; set; } = Vector3.One;
		public Vector3 Offset { get; set; }
		public Color Color { get; set; }
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

		public override void OnRender()
		{
			if(!init)
			{
				ModelsCache.Instance.CacheModel("cube", LoadModelFromMesh(GenMeshCube(1, 1, 1)));
				cube = ModelsCache.Instance.GetModel("cube");
				init = true;
			}
			DrawModelEx(cube, Entity.Transform.Position + Offset, new Vector3(0, 0, 1), 0, Size, Color);
		}
	}
}
