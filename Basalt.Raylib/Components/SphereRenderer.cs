using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Common.Physics;
using Basalt.Raylib.Graphics;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace Basalt.Raylib.Components
{
	public class SphereRenderer : Component
	{
		private Vector3 size;

		public Vector3 Size
		{
			get { return size; }
			set { size = value / 2; }
		}

		public int Rings { get; set; } = 16;
		public int Slices { get; set; } = 16;

		public Color Color = Color.Pink;

		public Vector3 Offset { get; set; } = Vector3.Zero;


		Model sphere;
		bool init;
		public SphereRenderer(Entity entity) : base(entity)
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
				if (!RaylibCache.Instance.HasModelKey("sphere"))
				{
					sphere = LoadModelFromMesh(GenMeshSphere(1, 16, 16));
					RaylibCache.Instance.CacheModel("sphere", sphere);
				}
				else
					sphere = RaylibCache.Instance.GetModel("sphere")!.Value;

				init = true;
			}
			DrawModelEx(sphere, Entity.Transform.Position + Offset, new Vector3(0, 0, 1), 0, Size, Color);
		}
	}
}
