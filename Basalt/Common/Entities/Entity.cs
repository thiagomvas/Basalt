using Basalt.Common.Components;
namespace Basalt.Common.Entities
{
	public class Entity
	{
		private HashSet<Component> components = new();
		public Transform Transform;
		public Rigidbody? Rigidbody;
		public List<Entity> Children { get; set; } = new();

		public bool IsActive = true;

		public Entity()
		{
			Transform = new Transform(this);
			AddComponent(Transform);
		}

		public void AddComponent(Component component)
		{
			components.Add(component);
			if(Rigidbody == null && component is Rigidbody rb)
			{
				Rigidbody = rb;
			}
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

		public void AddChildren(Entity e)
		{
			Children.Add(e);
		}

		public void RemoveChildren(Entity e)
		{
			Children.Remove(e);
		}
		public void Destroy()
		{
			Engine.RemoveEntity(this);
			foreach(var child in Children)
			{
				child.Destroy();
			}

			foreach(var component in components)
			{
				component.onDestroy(); // Call internal onDestroy method to do cleanup and call the overridable OnDestroy method
			}
		}
	}
}
