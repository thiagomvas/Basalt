using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Common
{
	public class Time
	{
		public float DeltaTime { get; set; }
		public float PhysicsDeltaTime { get; internal set; }
		private static Time instance;
		private static readonly object lockObject = new object();

		private Time()
		{
			// Private constructor to prevent instantiation
		}

		public static Time Instance
		{
			get
			{
				if (instance == null)
				{
					lock (lockObject)
					{
						if (instance == null)
						{
							instance = new Time();
						}
					}
				}
				return instance;
			}
		}
	}
}
