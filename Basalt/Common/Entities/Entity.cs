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

		public T? GetComponent<T>() where T : Component
		{
			foreach (var component in components)
			{
				if (component is T)
				{
					return (T)component;
				}
			}
			return null;
		}

		public List<Component> GetComponents()
		{
			return new List<Component>(components);
		}
		public void Destroy()
		{
			Engine.RemoveEntity(this);
		}
	}
}
