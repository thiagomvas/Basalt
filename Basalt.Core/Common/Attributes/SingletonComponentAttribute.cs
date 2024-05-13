using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Core.Common.Attributes
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class SingletonComponentAttribute : Attribute
	{
		public SingletonComponentAttribute()
		{
		}
	}
}
