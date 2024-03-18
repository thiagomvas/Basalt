using Basalt.Common.Entities;
using Basalt.Core.Common.Abstractions;
using Basalt.Core.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

		private Grid entityGrid = new(10);

		public float Gravity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public PhysicsEngine(ILogger? logger = null)
		{
			this.logger = logger;
		}
		public void Initialize()
		{
			logger?.LogInformation("Physics Engine Initialized");

			Engine.Instance.OnCreateEntity += OnCreateEntity;
			Simulate();

			logger?.LogWarning("Shut down physics engine");
		}

		public void Shutdown()
		{
			ShouldRun = false;
			logger?.LogWarning("Shutting down physics engine...");
		}

		public void Simulate()
		{
			while (ShouldRun)
			{
				startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

				Engine.Instance.EventBus?.NotifyPhysicsUpdate();

				entityGrid.Entities = Engine.Instance.EntityManager.GetEntities();

				entityGrid.UpdateGrid();

				elapsedTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() - startTime;
				Time.Instance.PhysicsDeltaTime = targetFrameTimeMs / 1000f;

				if (elapsedTime > targetFrameTimeMs)
				{
					logger?.LogWarning($"Physics engine is running behind. Elapsed time: {elapsedTime}ms");
				}

				if (elapsedTime < targetFrameTimeMs)
				{
					Task.Delay((int)(targetFrameTimeMs - elapsedTime)).Wait();
				}
			}
		}

		public void OnCreateEntity(Entity entity)
		{
		}
	}
}
