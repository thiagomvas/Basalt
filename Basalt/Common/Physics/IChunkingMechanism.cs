using Basalt.Common.Entities;
using System.Numerics;

namespace Basalt.Common.Physics
{
	public interface IChunkingMechanism
	{
		void AddEntity(Entity entity);
		void RemoveEntity(Entity entity);
		List<List<Entity>> GetEntitiesChunked();
		List<Entity> GetEntitiesNearPoint(Vector3 point);
		void Update();
	}
}