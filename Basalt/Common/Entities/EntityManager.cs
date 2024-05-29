using Basalt.Core.Common.Abstractions.Engine;

namespace Basalt.Common.Entities
{
	/// <summary>
	/// A thread safe class that contains and manages all entities active in the game world.
	/// </summary>
	public class EntityManager
	{
		public int EntityCount => entities.Count;
		private List<Entity> entities = new List<Entity>();
		private readonly object lockObject = new object();
		private readonly IEventBus eventBus;

		private List<Entity> queuedEntities = new List<Entity>();

		public EntityManager(IEventBus eventBus)
		{
			this.eventBus = eventBus;
		}

		/// <summary>
		/// Adds an entity to the entity manager.
		/// </summary>
		/// <param name="entity">The entity to add</param>
		public void AddEntity(Entity entity)
		{

			lock (lockObject)
			{
				Engine.Instance.Logger?.LogDebug($"Adding entity {entity.Id} to the entity manager.");
				entities.Add(entity);

				Engine.Instance.Logger?.LogDebug($"Added entity {entity.Id} to the entity manager.");
			}
		}

		/// <summary>
		/// Removes an entity from the entity manager.
		/// </summary>
		/// <param name="entity">The entity to remove</param>
		public void RemoveEntity(Entity entity)
		{
			lock (lockObject)
			{
				entities.Remove(entity);
				foreach (var component in entity.GetComponents())
					component.OnDestroy();
			}
		}

		/// <summary>
		/// Gets all entities in the entity manager.
		/// </summary>
		/// <returns>A list containing all the entities</returns>
		public List<Entity> GetEntities()
		{
			lock (lockObject)
			{
				return entities;
			}
		}

		/// <summary>
		/// Gets an entity with a specified ID
		/// </summary>
		/// <param name="id">The id of the entity</param>
		/// <returns>An entity whose id matches the specified <paramref name="id"/>, returns null if none were found</returns>
		public Entity? GetEntity(string id)
		{
			lock (lockObject)
			{
				return entities.FirstOrDefault(e => e.Id == id);
			}
		}
	}
}
