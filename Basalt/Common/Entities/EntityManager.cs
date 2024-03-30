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
	}
}
