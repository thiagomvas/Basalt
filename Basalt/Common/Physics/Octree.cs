using Basalt.Common.Entities;
using System.Numerics;

namespace Basalt.Common.Physics
{
	internal class Octree : IChunkingMechanism
	{
		private class Node
		{
			public Vector3 Position { get; }
			public float Size { get; }
			public List<Entity> Entities { get; }
			public Node[] Children { get; }

			public Node(Vector3 position, float size)
			{
				Position = position;
				Size = size;
				Entities = new List<Entity>();
				Children = new Node[8];
			}
		}

		private Node root;
		private int maxEntitiesPerNode;
		private float minNodeSize;
		private Dictionary<Entity, Node> entityNodeMap;
		private HashSet<Entity> entitiesToUpdate;

		public Octree(Vector3 position, float size, int maxEntitiesPerNode = 4, float minNodeSize = 1.0f)
		{
			root = new Node(position, size);
			this.maxEntitiesPerNode = maxEntitiesPerNode;
			this.minNodeSize = minNodeSize;
			entityNodeMap = new Dictionary<Entity, Node>();
			entitiesToUpdate = new HashSet<Entity>();
		}

		public void AddEntity(Entity entity)
		{
			AddEntity(root, entity);
		}

		private void AddEntity(Node node, Entity entity)
		{
			if (node.Size / 2 < minNodeSize || node.Entities.Count < maxEntitiesPerNode)
			{
				node.Entities.Add(entity);
				entityNodeMap[entity] = node;
				return;
			}

			int index = GetChildIndex(node, entity.Transform.Position);
			if (node.Children[index] == null)
			{
				Vector3 newPos = GetChildPosition(node, index);
				node.Children[index] = new Node(newPos, node.Size / 2);
			}

			AddEntity(node.Children[index], entity);
		}

		public void RemoveEntity(Entity entity)
		{
			if (entityNodeMap.TryGetValue(entity, out Node node))
			{
				node.Entities.Remove(entity);
				entityNodeMap.Remove(entity);
			}
		}

		public List<List<Entity>> GetEntitiesChunked()
		{
			List<List<Entity>> chunks = new List<List<Entity>>();
			GetEntitiesChunked(root, chunks);
			return chunks;
		}

		private void GetEntitiesChunked(Node node, List<List<Entity>> chunks)
		{
			if (node == null) return;

			if (node.Entities.Count > 0)
			{
				chunks.Add(new List<Entity>(node.Entities));
			}

			foreach (var child in node.Children)
			{
				GetEntitiesChunked(child, chunks);
			}
		}

		public List<Entity> GetEntitiesNearPoint(Vector3 point)
		{
			List<Entity> entities = new List<Entity>();
			GetEntitiesNearPoint(root, point, entities);
			return entities;
		}

		private void GetEntitiesNearPoint(Node node, Vector3 point, List<Entity> entities)
		{
			if (node == null) return;

			if (Vector3.Distance(node.Position, point) <= node.Size)
			{
				entities.AddRange(node.Entities);
			}

			foreach (var child in node.Children)
			{
				GetEntitiesNearPoint(child, point, entities);
			}
		}

		public void Update()
		{
			foreach (var entity in entitiesToUpdate)
			{
				RemoveEntity(entity);
				AddEntity(entity);
			}
			entitiesToUpdate.Clear();
		}

		public void MarkForUpdate(Entity entity)
		{
			entitiesToUpdate.Add(entity);
		}

		private int GetChildIndex(Node node, Vector3 position)
		{
			int index = 0;
			if (position.X > node.Position.X) index |= 1;
			if (position.Y > node.Position.Y) index |= 2;
			if (position.Z > node.Position.Z) index |= 4;
			return index;
		}

		private Vector3 GetChildPosition(Node node, int index)
		{
			float offsetX = (index & 1) == 0 ? -node.Size / 4 : node.Size / 4;
			float offsetY = (index & 2) == 0 ? -node.Size / 4 : node.Size / 4;
			float offsetZ = (index & 4) == 0 ? -node.Size / 4 : node.Size / 4;
			return new Vector3(node.Position.X + offsetX, node.Position.Y + offsetY, node.Position.Z + offsetZ);
		}
	}
}
