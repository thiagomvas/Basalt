using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Common.Entities
{
	/// <summary>
	/// A thread safe class that contains and manages all entities active in the game world.
	/// </summary>
	public class EntityManager
	{
		private List<Entity> entities = new List<Entity>();
		private readonly object lockObject = new object();

		/// <summary>
		/// Adds an entity to the entity manager.
		/// </summary>
		/// <param name="entity">The entity to add</param>
		public void AddEntity(Entity entity)
		{
			lock (lockObject)
			{
				entities.Add(entity);
				foreach(var component in entity.GetComponents())
					Engine.Instance.EventBus?.Subscribe(component);
				foreach (var child in entity.Children)
					AddEntity(child);
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
					Engine.Instance.EventBus?.Unsubscribe(component);
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
				return new List<Entity>(entities);
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
