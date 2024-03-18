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

namespace Basalt.Raylib.Components
{
	public class BoxRenderer : Component
	{
		public Vector3 Size { get; set; } = Vector3.One;
		public Vector3 Offset { get; set; }
		public Color Color { get; set; }
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
			DrawCubeV(entity.Transform.Position + Offset, Size, Color);
		}
	}
}
