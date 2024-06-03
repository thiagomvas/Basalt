using Basalt.Common.Entities;
using System.Numerics;

namespace Basalt.Common.Physics
{
	public class Grid : IChunkingMechanism
	{
		private List<Entity> entities = new();
		private Queue<Entity> entityAddQueue = new Queue<Entity>();
		private Queue<Entity> entityRemoveQueue = new Queue<Entity>();

		private Dictionary<Point, List<Entity>> chunks = new Dictionary<Point, List<Entity>>();
		private readonly int sideLength;

		private readonly object lockObject = new object();

		public Grid(int sideLength)
		{
			this.sideLength = sideLength;
		}
		public List<Entity> GetEntities()
		{
			return entities;
		}
		public void AddEntity(Entity entity)
		{
			lock (lockObject)
			{
				entityAddQueue.Enqueue(entity);
			}
		}

		public void RemoveEntity(Entity entity)
		{
			lock (lockObject)
			{

				entityRemoveQueue.Enqueue(entity);
			}
		}

		public void Update()
		{
			lock (lockObject)
			{
				while (entityRemoveQueue.Count > 0)
				{
					var entity = entityRemoveQueue.Dequeue();
					if (entities.Contains(entity))
					{
						entities.Remove(entity);
						RemoveEntityFromChunks(entity);
					}
				}

				while (entityAddQueue.Count > 0)
				{
					var entity = entityAddQueue.Dequeue();
					if (!entities.Contains(entity))
					{
						entities.Add(entity);
						AddEntityToChunks(entity);
					}
				}
			}

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
				var baseChunk = GetChunk(chunk.FirstOrDefault()?.Transform.Position ?? Vector3.Zero);
				// Add adjacent chunks
				for (int x = -1; x <= 1; x++)
				{
					for (int z = -1; z <= 1; z++)
					{
						var adjacentChunk = new Point(baseChunk.X + x, baseChunk.Z + z);
						if (chunks.ContainsKey(adjacentChunk))
						{
							chunkedEntities.Add(new List<Entity>(chunks[adjacentChunk]));
						}
					}

				}
			};
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
			lock (lockObject)
			{
				entityAddQueue.Enqueue(entity);
				entityRemoveQueue.Enqueue(entity);
			}
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
