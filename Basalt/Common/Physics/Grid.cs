using Basalt.Common.Entities;
using System.Numerics;

namespace Basalt.Common.Physics
{
	public class Grid : IChunkingMechanism
	{
		private List<Entity> Entities = new List<Entity>();
		private List<Entity> entityAddQueue = new List<Entity>();
		private List<Entity> entityRemoveQueue = new List<Entity>();

		Dictionary<Point, List<Entity>> chunks = new Dictionary<Point, List<Entity>>();

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
			chunks.Clear();
			Entities.AddRange(entityAddQueue);
			foreach(var entity in entityRemoveQueue)
			{
				Entities.Remove(entity);
			}
			entityAddQueue.Clear();
			entityRemoveQueue.Clear();

			foreach (var entity in Entities)
			{
				var position = entity.Transform.Position;

				var chunk = new Point((int)position.X / sideLength, (int)position.Z / sideLength);

				if (!chunks.ContainsKey(chunk))
				{
					chunks.Add(chunk, new List<Entity>());
				}

				chunks[chunk].Add(entity);
			}
		}

		public List<Entity> GetEntitiesNearPoint(Vector3 point)
		{


			List<Entity> e = new();

			for (int x = -1; x <= 1; x++)
			{
				for (int z = -1; z <= 1; z++)
				{
					var chunk = new Point((int)point.X / sideLength + x, (int)point.Z / sideLength + z);

					if (chunks.ContainsKey(chunk))
						e.AddRange(chunks[chunk]);
				}
			}

			return e;
		}

		public List<List<Entity>> GetEntitiesChunked()
		{
			List<List<Entity>> chunkedEntities = new List<List<Entity>>();

			foreach (var position in chunks.Keys)
			{
				for (int x = -1; x <= 1; x++)
				{
					for (int z = -1; z <= 1; z++)
					{
						var chunk = new Point((int)position.X / sideLength + x, (int)position.Z / sideLength + z);

						if (chunks.ContainsKey(chunk))
						{
							chunkedEntities.Add(chunks[chunk]);
						}
						else
						{
							chunkedEntities.Add(new List<Entity>());
						}
					}
				}
			}

			return chunkedEntities;
		}

		private struct Point(int X, int Z)
		{
			public int X { get; } = X;
			public int Z { get; } = Z;
			public override string ToString()
			{
				return $"{X}, {Z}";
			}
		}
	}
}
