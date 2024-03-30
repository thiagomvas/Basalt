using Basalt.Common.Entities;
using System.Numerics;

namespace Basalt.Common.Physics
{
	public class Grid
	{
		public List<Entity> Entities = new List<Entity>();

		Dictionary<Point, List<Entity>> chunks = new Dictionary<Point, List<Entity>>();

		private readonly int sideLength;
		public Grid(int sideLength)
		{
			this.sideLength = sideLength;
		}

		public void AddEntity(Entity entity)
		{
			Entities.Add(entity);
		}

		public void UpdateGrid()
		{
			chunks.Clear();

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

			for(int x = -1; x <= 1; x++)
			{
				for(int z = -1; z <= 1; z++)
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
			public int X { get; }
			public int Z { get; }
			public override string ToString()
			{
				return $"{X}, {Z}";
			}
		}
	}
}
