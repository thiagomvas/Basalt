using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Core.Common.Abstractions.Engine;

namespace Basalt.Common.Physics
{
    /// <summary>
    /// Default implementation for a basic physics engine.
    /// </summary>
    public class PhysicsEngine : IPhysicsEngine
	{
		private readonly ILogger? logger;
		private bool ShouldRun = true;

		public long startTime, elapsedTime;

		const int targetFrameTimeMs = 16;

		private Grid entityGrid = new(10);
		private IEventBus eventBus;
		

		/// <summary>
		/// Gets or sets the gravity value for the physics engine.
		/// </summary>
		public float Gravity { get; set; } = 9.81f;


		/// <summary>
		/// Initializes the physics engine.
		/// </summary>
		public void Initialize()
		{
			var bus = Engine.Instance.GetEngineComponent<IEventBus>();
			if(bus == null)
			{
				logger?.LogError("Could not find an event bus component that implements IEventBus. Cannot run without one.");
				return;
			}
			eventBus = bus;
			logger?.LogInformation("Physics Engine Initialized");

			Simulate();

			logger?.LogWarning("Shut down physics engine");
		}

		/// <summary>
		/// Shuts down the physics engine.
		/// </summary>
		public void Shutdown()
		{
			ShouldRun = false;
			logger?.LogWarning("Shutting down physics engine...");
		}

		/// <summary>
		/// Simulates the physics interactions.
		/// </summary>
		public void Simulate()
		{
			while (ShouldRun)
			{
				startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

				eventBus?.NotifyPhysicsUpdate();

				entityGrid.Entities = Engine.Instance.EntityManager.GetEntities();

				entityGrid.Update();

				// Check for collisions
				foreach (var chunk in entityGrid.GetEntitiesChunked())
				{
					for (int i = 0; i < chunk.Count; i++)
					{
						for (int j = i + 1; j < chunk.Count; j++)
						{
							var entityA = chunk[i];
							var entityB = chunk[j];

							var colliderA = entityA.GetComponent<Collider>();
							var colliderB = entityB.GetComponent<Collider>();

							if (colliderA != null && colliderB != null)
							{
								CollisionHandler.Handle(colliderA, colliderB);
							}
						}
					}
				}

				elapsedTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() - startTime;
				Time.PhysicsDeltaTime = targetFrameTimeMs / 1000f;

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

	}
}
