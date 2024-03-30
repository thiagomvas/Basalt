using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Core.Common.Attributes
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class UniqueComponentAttribute : Attribute
	{
		private static object _lock = new object();
		private static bool _instanceCreated = false;

		public UniqueComponentAttribute()
		{
			lock (_lock)
			{
				if (_instanceCreated)
				{
					return;
				}
				_instanceCreated = true;
			}
		}
	}
}
