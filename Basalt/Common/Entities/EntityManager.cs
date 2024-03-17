using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Common.Entities
{
	public class EntityManager
	{
		private List<Entity> entities = new List<Entity>();
		private readonly object lockObject = new object();

		public void AddEntity(Entity entity)
		{
			lock (lockObject)
			{
				entities.Add(entity);
			}
		}

		public void RemoveEntity(Entity entity)
		{
			lock (lockObject)
			{
				entities.Remove(entity);
			}
		}

		public List<Entity> GetEntities()
		{
			lock (lockObject)
			{
				return new List<Entity>(entities);
			}
		}
	}
}
