using Basalt.Common.Entities;
using Basalt.Types;
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
		public Vector3 Size = Vector3.One, Offset = Vector3.Zero;
		public BoxCollider(Entity entity) : base(entity)
		{

		}

		public override void OnStart()
		{
		}

		public override void OnUpdate() { }
	}

}
