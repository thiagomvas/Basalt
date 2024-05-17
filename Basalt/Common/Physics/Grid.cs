using Basalt.Common.Entities;
using System.Numerics;

namespace Basalt.Common.Physics
{
	public class Grid : IChunkingMechanism
	{
		private List<Entity> entities = new List<Entity>();
		private List<Entity> entityAddQueue = new List<Entity>();
		private List<Entity> entityRemoveQueue = new List<Entity>();
		private HashSet<Entity> entitiesToUpdate = new HashSet<Entity>();

		private Dictionary<Point, List<Entity>> chunks = new Dictionary<Point, List<Entity>>();
		private readonly int sideLength;

		public Grid(int sideLength)
		{
			this.sideLength = sideLength;
		}

		public void AddEntity(Entity entity)
		{
			entityAddQueue.Add(entity);
		}

		public void RemoveEntity(Entity entity)
		{
			entityRemoveQueue.Add(entity);
		}

		public void Update()
		{
			foreach (var entity in entityRemoveQueue)
			{
				if (entities.Contains(entity))
				{
					RemoveEntityFromChunks(entity);
					entities.Remove(entity);
				}
			}

			entityRemoveQueue.Clear();

			foreach (var entity in entityAddQueue)
			{
				entities.Add(entity);
				AddEntityToChunks(entity);
			}

			entityAddQueue.Clear();

			foreach (var entity in entitiesToUpdate)
			{
				if (entities.Contains(entity))
				{
					RemoveEntityFromChunks(entity);
					AddEntityToChunks(entity);
				}
			}

			entitiesToUpdate.Clear();
		}

		private void AddEntityToChunks(Entity entity)
		{
			var chunk = GetChunk(entity.Transform.Position);
			if (!chunks.ContainsKey(chunk))
			{
				chunks.Add(chunk, new List<Entity>());
			}
			chunks[chunk].Add(entity);
		}

		private void RemoveEntityFromChunks(Entity entity)
		{
			var chunk = GetChunk(entity.Transform.Position);
			if (chunks.ContainsKey(chunk))
			{
				chunks[chunk].Remove(entity);
				if (chunks[chunk].Count == 0)
				{
					chunks.Remove(chunk);
				}
			}
		}

		public List<List<Entity>> GetEntitiesChunked()
		{
			List<List<Entity>> chunkedEntities = new List<List<Entity>>();
			foreach (var chunk in chunks.Values)
			{
				chunkedEntities.Add(new List<Entity>(chunk));
			}
			return chunkedEntities;
		}

		public List<Entity> GetEntitiesNearPoint(Vector3 point)
		{
			List<Entity> nearbyEntities = new List<Entity>();
			var baseChunk = GetChunk(point);

			for (int x = -1; x <= 1; x++)
			{
				for (int z = -1; z <= 1; z++)
				{
					var chunk = new Point(baseChunk.X + x, baseChunk.Z + z);
					if (chunks.ContainsKey(chunk))
					{
						nearbyEntities.AddRange(chunks[chunk]);
					}
				}
			}
			return nearbyEntities;
		}

		public void MarkForUpdate(Entity entity)
		{
			entitiesToUpdate.Add(entity);
		}

		private Point GetChunk(Vector3 position)
		{
			return new Point((int)position.X / sideLength, (int)position.Z / sideLength);
		}

		private struct Point
		{
			public int X { get; }
			public int Z { get; }

			public Point(int x, int z)
			{
				X = x;
				Z = z;
			}

			public override string ToString()
			{
				return $"{X}, {Z}";
			}
		}
	}
}
