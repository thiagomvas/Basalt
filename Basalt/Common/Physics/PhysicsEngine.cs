using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Common.Utils;
using Basalt.Core.Common.Abstractions.Engine;

namespace Basalt.Common.Physics
{
	/// <summary>
	/// Default implementation for a basic physics engine.
	/// </summary>
	public class PhysicsEngine : IPhysicsEngine
	{

		public long startTime, elapsedTime;

		const float targetDeltaTime = 0.02f;
		const int targetFrameTimeMs = 20;

		internal IChunkingMechanism chunking;
		private IEventBus eventBus;
		private ILogger? logger;
		private bool ShouldRun = true;
		private List<Entity> entities;
		/// <summary>
		/// Gets or sets the gravity value for the physics engine.
		/// </summary>
		public float Gravity { get; set; } = 9.81f;

		/// <summary>
		/// Initializes a new instance of the <see cref="CustomPhysics"/> class using <see cref="Grid"/> as a <see cref="IChunkingMechanism"/>.
		/// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public PhysicsEngine()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		{
			chunking = new Grid(32);
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="CustomPhysics"/> class using the specified <see cref="IChunkingMechanism"/>.
		/// </summary>
		/// <param name="chunkingMechanism">The chunking mechanism used to optimize collision handling</param>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public PhysicsEngine(IChunkingMechanism chunkingMechanism)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		{
			chunking = chunkingMechanism;
		}

		/// <summary>
		/// Initializes the physics engine.
		/// </summary>
		public void Initialize()
		{
			var bus = Engine.Instance.GetEngineComponent<IEventBus>();
			logger = Engine.Instance.Logger;
			if (bus == null)
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

				eventBus?.TriggerEvent(BasaltConstants.PhysicsUpdateEventKey);

				chunking.Update();

				// Check for collisions

				DetectCollisions(chunking.GetEntitiesChunked());

				elapsedTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() - startTime;

				if (elapsedTime > targetFrameTimeMs)
				{
					logger?.LogWarning($"Physics engine is running behind. Elapsed time: {elapsedTime}ms");
					Time.PhysicsDeltaTime = elapsedTime * 0.001f;
					continue;
				}

				if (elapsedTime < targetFrameTimeMs)
				{
					Task.Delay((int)(targetFrameTimeMs - elapsedTime)).Wait();
					Time.PhysicsDeltaTime = targetDeltaTime;
				}
			}
		}
		public void DetectCollisions(List<List<Entity>> chunks)
		{

			// Use Parallel.For for optimized collision detection
			Parallel.ForEach(chunks, (chunk) =>
			{
				for (int i = 0; i < chunk.Count; i++)
				{
					Entity entityA = chunk[i];

					for (int j = i + 1; j < chunk.Count; j++)
					{
						Entity entityB = chunk[j];

						var colliderA = entityA.Collider;
						var colliderB = entityB.Collider;

						if (colliderA != null && colliderB != null)
						{
							CollisionHandler.Handle(colliderA, colliderB);
						}
					}
				}
			});
		}

		private void detectCollisionFlattened(List<Entity> entities)
		{
			int totalEntities = entities.Count;

			// Use Parallel.For for optimized collision detection
			Parallel.For(0, totalEntities, i =>
			{
				Entity entityA = entities[i];

				for (int j = i + 1; j < totalEntities; j++)
				{
					Entity entityB = entities[j];

					var colliderA = entityA.Collider;
					var colliderB = entityB.Collider;

					if (colliderA != null && colliderB != null)
					{
						CollisionHandler.Handle(colliderA, colliderB);
					}
				}
			});
		}

		public void AddEntityToSimulation(object entity)
		{
			if (entity is Entity e)
			{
				chunking.AddEntity(e);
			}
		}

		public void RemoveEntityFromSimulation(object entity)
		{
			if (entity is Entity e)
			{
				chunking.RemoveEntity(e);
			}
		}
	}
}
