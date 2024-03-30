using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Core.Common.Abstractions;

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

		/// <summary>
		/// Gets or sets the gravity value for the physics engine.
		/// </summary>
		public float Gravity { get; set; } = 9.81f;

		/// <summary>
		/// Initializes a new instance of the <see cref="PhysicsEngine"/> class.
		/// </summary>
		/// <param name="logger">The logger to use for logging messages.</param>
		public PhysicsEngine(ILogger? logger = null)
		{
			this.logger = logger;
		}

		/// <summary>
		/// Initializes the physics engine.
		/// </summary>
		public void Initialize()
		{
			logger?.LogInformation("Physics Engine Initialized");

			Engine.Instance.OnCreateEntity += OnCreateEntity;
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

				Engine.Instance.EventBus?.NotifyPhysicsUpdate();

				entityGrid.Entities = Engine.Instance.EntityManager.GetEntities();

				entityGrid.UpdateGrid();

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

		/// <summary>
		/// Handles the creation of an entity.
		/// </summary>
		/// <param name="entity">The entity that was created.</param>
		public void OnCreateEntity(Entity entity)
		{
		}
	}
}
