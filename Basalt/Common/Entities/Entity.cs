using Basalt.Common.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
namespace Basalt.Common.Entities
{
	public class ComponentDto
	{
		public Type Type { get; set; }
		public Component Data { get; set; }
	}
	public class Entity
	{
		[JsonProperty("Components")]
		private List<ComponentDto> componentDtos = new();
		private HashSet<Component> components = new();

		[JsonIgnore]
		public Transform Transform;

		[JsonIgnore]
		public Rigidbody? Rigidbody;
		public List<Entity> Children { get; set; } = new();

		public bool IsActive = true;

		public Entity()
		{
			Transform = new Transform(this);
			AddComponent(Transform);
		}

		public string SerializeToJson()
		{
			componentDtos = new();
			foreach (var component in components)
			{
				componentDtos.Add(new ComponentDto
				{
					Type = component.GetType(),
					Data = component
				});
			}
			return JsonConvert.SerializeObject(this, Formatting.Indented);

		}

		public static Entity DeserializeFromJson(string json)
		{
			JObject jObject = JObject.Parse(json);

			var target = new Entity();

			foreach (var component in jObject["Components"])
			{
				var type = ByName(component["Type"].Value<string>().Split(',').First());

				ConstructorInfo constructor = type.GetConstructor(new[] { typeof(Entity) });

				var typeProps = type.GetProperties();
				var instance = constructor.Invoke([target]);

				foreach (var prop in component["Data"] as JObject)
				{
					if (typeProps.Any(p => p.Name == prop.Key))
					{
						var propInfo = typeProps.First(p => p.Name == prop.Key);
						if (propInfo.CanWrite)
						{
							propInfo.SetValue(instance, prop.Value.ToObject(propInfo.PropertyType));
						}
					}
				}

				target.AddComponent(instance as Component);
			}

			return target;

		}

		public void AddComponent(Component component)
		{
			components.Add(component);
			if(Rigidbody == null && component is Rigidbody rb)
			{
				Rigidbody = rb;
			}

			else if(component is Transform t)
			{
				Transform = t;
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


		private static Type ByName(string name)
		{
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Reverse())
			{
				var tt = assembly.GetType(name);
				if (tt != null)
				{
					return tt;
				}
			}

			return null;
		}
	}
}
