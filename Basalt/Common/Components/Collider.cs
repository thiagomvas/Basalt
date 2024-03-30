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
	public abstract class Collider : Component
	{
		public Vector3 Offset;
		public Vector3 Position => Entity.Transform.Position + Offset;
		protected Collider(Entity entity) : base(entity)
		{
		}
	}
}
