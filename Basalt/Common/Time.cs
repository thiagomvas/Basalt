namespace Basalt.Common
{
	/// <summary>
	/// Represents a class for managing time-related operations.
	/// </summary>
	public class Time
	{
		private float deltaTime;
		private float physicsDeltaTime;

		/// <summary>
		/// Gets or sets the time between the current frame and the previous frame.
		/// </summary>
		public static float DeltaTime
		{
			get => Instance.deltaTime;
			set => Instance.deltaTime = value;
		}

		/// <summary>
		/// Gets or sets the time between the current physics update and the previous physics update.
		/// </summary>
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

		/// <summary>
		/// Gets the singleton instance of the Time class.
		/// </summary>
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
