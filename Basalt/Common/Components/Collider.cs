using Basalt.Common.Entities;
using Basalt.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Common.Components
{
	public abstract class Collider : Component
	{
		public ColliderType Type { get; private set; }
		protected Collider(Entity entity) : base(entity)
		{
		}
	}
}
