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

	/// <summary>
	/// Represents an entity in the game world, containing components and children entities.
	/// </summary>
	public class Entity
	{
		[JsonProperty("Components")]
		private List<ComponentDto> componentDtos = new();
		private HashSet<Component> components = new();

		[JsonProperty("Id")]
		public string Id { get; set; } = System.Guid.NewGuid().ToString();
		/// <summary>
		/// The transform component of the entity.
		/// </summary>
		[JsonIgnore]
		public Transform Transform;

		/// <summary>
		/// The rigidbody component of the entity.
		/// </summary>
		[JsonIgnore]
		public Rigidbody? Rigidbody;

		[JsonIgnore]
		public Entity? Parent { get; private set; }

		/// <summary>
		/// The children entities of the entity.
		/// </summary>
		public List<Entity> Children { get; set; } = new();


		/// <summary>
		/// Whether the entity is active or not.
		/// </summary>
		public bool IsActive = true;

		public Entity()
		{
			Transform = new Transform(this);
			AddComponent(Transform);
		}

		/// <summary>
		/// Serializes the entity to a JSON string.
		/// </summary>
		/// <returns>The entity and it's data in JSON format</returns>
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

			List<JObject> childrenObjects = new List<JObject>();

			foreach (var child in Children)
			{
				JObject childObject = JObject.Parse(child.SerializeToJson());
				childrenObjects.Add(childObject);
			}

			var entityJson = new JObject();
			entityJson["Components"] = JArray.FromObject(componentDtos);
			entityJson["Children"] = JArray.FromObject(childrenObjects); // Use the parsed child objects
			entityJson["Id"] = Id;

			return entityJson.ToString(Formatting.Indented);
		}

		/// <summary>
		/// Deserializes an entity from a JSON string.
		/// </summary>
		/// <param name="json">The json string to deserialize from</param>
		/// <returns>An entity instance from the JSON string</returns>
		public static Entity DeserializeFromJson(string json)
		{
			JObject jObject = JObject.Parse(json);

			var target = new Entity();
			target.Id = jObject["Id"].Value<string>();

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

			foreach (var child in jObject["Children"])
			{
				var childEntity = DeserializeFromJson(child.ToString());
				target.AddChildren(childEntity);
			}

			return target;

		}

		/// <summary>
		/// Adds a component to the entity.
		/// </summary>
		/// <param name="component">The component to add</param>
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
		
		/// <summary>
		/// Removes a component from the entity.
		/// </summary>
		/// <param name="component">The component reference to remove</param>
		public void RemoveComponent(Component component)
		{
			components.Remove(component);
		}

		/// <summary>
		/// Gets a component of the specified type from the entity.
		/// </summary>
		/// <typeparam name="T">The type of the component</typeparam>
		/// <returns>The first instance of a component of type <typeparamref name="T"/></returns>
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

		/// <summary>
		/// Gets all components of the entity.
		/// </summary>
		/// <returns>A list of all components in this entity</returns>
		public List<Component> GetComponents()
		{
			return new List<Component>(components);
		}

		/// <summary>
		/// Adds a child entity to the entity.
		/// </summary>
		/// <param name="child">The child entity to add</param>
		public void AddChildren(Entity child)
		{
			Children.Add(child);
			child.Parent = this;
		}

		/// <summary>
		/// Removes a child entity from the entity.
		/// </summary>
		/// <param name="child">The child entity reference to remove</param>
		public void RemoveChildren(Entity child)
		{
			Children.Remove(child);
		}

		/// <summary>
		/// Destroys the entity and all of its children.
		/// </summary>
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

		internal void CallOnStart()
		{
			foreach (var component in components)
			{
				component.OnStart();
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
