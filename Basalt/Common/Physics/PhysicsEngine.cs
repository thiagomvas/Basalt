using Basalt.Core.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Common.Physics
{
	public class PhysicsEngine : IPhysicsEngine
	{
		private readonly ILogger? logger;
		private bool ShouldRun = true;

		public long startTime, elapsedTime;

		const int targetFrameTimeMs = 16;

		public PhysicsEngine(ILogger? logger = null)
		{
			this.logger = logger;
		}
		public void Initialize()
		{
			logger?.LogInformation("Physics Engine Initialized");
			while(ShouldRun)
			{
				startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

				Engine.Instance.EventBus?.NotifyPhysicsUpdate();


				Time.Instance.DeltaTime = elapsedTime;
				Time.Instance.PhysicsDeltaTime = targetFrameTimeMs;

				elapsedTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() - startTime;

				if(elapsedTime > targetFrameTimeMs)
				{
					logger?.LogWarning($"Physics engine is running behind. Elapsed time: {elapsedTime}ms");
				}

				if(elapsedTime < targetFrameTimeMs)
				{
					Task.Delay((int)(targetFrameTimeMs - elapsedTime)).Wait();
				}
			}
			logger?.LogWarning("Shut down physics engine");
		}

		public void Shutdown()
		{
			ShouldRun = false;
			logger?.LogWarning("Shutting down physics engine...");
		}
	}
}
