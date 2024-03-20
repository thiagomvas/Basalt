using Basalt.Common.Entities;
using Basalt.Types;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Common.Components
{
	public class BoxCollider : Collider
	{
		public Vector3 Size = Vector3.One;
		Vector3 pos => Entity.Transform.Position + Offset;
		public BoxCollider(Entity entity) : base(entity)
		{

		}

		public override void OnStart()
		{
		}

		public override void OnUpdate() { }

		public override void OnRender()
		{
			Raylib.DrawCubeWires(Position, Size.X, Size.Y, Size.Z, Color.Pink);
		}
	}

}
