using Basalt.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
			var chunk = new Point((int)point.X / sideLength, (int)point.Z / sideLength);


			List<Entity> e = new();

			if (chunks.ContainsKey(chunk))
				e.AddRange(chunks[chunk]);

			return e;
		}

		public List<List<Entity>> GetEntitiesChunked()
		{
			return chunks.Values.ToList();
		}

		private struct Point(int X, int Z)
		{
			public override string ToString()
			{
				return $"{X}, {Z}";
			}
		}
	}
}
