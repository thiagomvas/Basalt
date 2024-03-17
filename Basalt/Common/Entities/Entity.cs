using Basalt.Common.Components;
namespace Basalt.Common.Entities
{
	public class Entity
	{
		private HashSet<Component> components = new();
		public Transform Transform;

		public Entity()
		{
			Transform = new Transform(this);
			AddComponent(Transform);
		}

		public void AddComponent(Component component)
		{
			components.Add(component);
		}

		public void RemoveComponent(Component component)
		{
			components.Remove(component);
		}

		public List<Component> GetComponents()
		{
			return new List<Component>(components);
		}
	}
}
