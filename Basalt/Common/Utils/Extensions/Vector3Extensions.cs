using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Common.Utils.Extensions
{
	public static class Vector3Extensions
	{
		public static Vector3 XZNormalized(this Vector3 vector)
		{
			return Vector3.Normalize(new Vector3(vector.X, 0, vector.Z));
		}
	}
}
