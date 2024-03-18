using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Common
{
	public class Time
	{
		private float deltaTime;
		private float physicsDeltaTime;

		public static float DeltaTime
		{
			get => Instance.deltaTime;
			set => Instance.deltaTime = value;
		}

		public static float PhysicsDeltaTime
		{
			get => Instance.physicsDeltaTime;
			set => Instance.physicsDeltaTime = value;
		}

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
