using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Common.Physics;
using Basalt.Raylib.Graphics;
using Raylib_cs;
using System.Numerics;

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
				if(RaylibCache.Instance.HasModelKey("sphere"))
					sphere = RaylibCache.Instance.GetModel("sphere")!.Value;

				else
				{
					Model s = Raylib_cs.Raylib.LoadModelFromMesh(Raylib_cs.Raylib.GenMeshSphere(1, Rings, Slices));
					RaylibCache.Instance.CacheModel("sphere", s);
				}
				
				init = true;
			}
			Raylib_cs.Raylib.DrawModelEx(sphere, Entity.Transform.Position + Offset, new Vector3(0, 0, 1), 0, Size, Color);
		}
	}
}
