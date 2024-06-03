using Newtonsoft.Json;
namespace Basalt.Common.Utils
{
	/// <summary>
	/// Represents a cache for storing and retrieving resources.
	/// </summary>
	public class ResourceCache
	{
		private static ResourceCache instance;
		private static readonly object padlock = new object();
		private Dictionary<string, object> resourceCache;

		private ResourceCache()
		{
			resourceCache = new Dictionary<string, object>();
		}

		/// <summary>
		/// Gets the singleton instance of the ResourceCache class.
		/// </summary>
		public static ResourceCache Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ResourceCache();
				}
				return instance;
			}
		}

		/// <summary>
		/// Caches a resource with the specified name.
		/// </summary>
		/// <param name="resourceName">The name of the resource.</param>
		/// <param name="resource">The resource to cache.</param>
		public static void CacheResource(string resourceName, object resource)
		{
			resourceName = resourceName.ToLower().Trim();
			lock (padlock)
			{
				if (!Instance.resourceCache.ContainsKey(resourceName))
				{
					Instance.resourceCache.Add(resourceName, resource);
				}
			}
		}

		/// <summary>
		/// Gets a resource with the specified name.
		/// </summary>
		/// <typeparam name="T">The type of the resource.</typeparam>
		/// <param name="resourceName">The name of the resource.</param>
		/// <returns>The resource with the specified name, or the default value if not found.</returns>
		public static T? GetResource<T>(string resourceName)
		{
			resourceName = resourceName.ToLower().Trim();
			lock (padlock)
			{
				if (Instance.resourceCache.ContainsKey(resourceName) && TryGetResource(resourceName, out T result))
				{
					return result;
				}
				else
				{
					return default;
				}
			}
		}

		/// <summary>
		/// Tries to get a resource with the specified name.
		/// </summary>
		/// <typeparam name="T">The type of the resource.</typeparam>
		/// <param name="resourceName">The name of the resource.</param>
		/// <param name="resource">When this method returns, contains the resource with the specified name, if found; otherwise, the default value.</param>
		/// <returns>true if the resource with the specified name is found; otherwise, false.</returns>
		public static bool TryGetResource<T>(string resourceName, out T resource)
		{
			resourceName = resourceName.ToLower().Trim();
			lock (padlock)
			{
				if (Instance.resourceCache.ContainsKey(resourceName))
				{
					var r = Instance.resourceCache[resourceName];
					if(r is T t)
					{
						resource = t;
						return true;
					}
					resource = default;
					return false;
				}
				else
				{
					resource = default;
					return false;
				}
			}
		}

		/// <summary>
		/// Loads a resource from a file and caches it with the specified name.
		/// </summary>
		/// <param name="resourceName">The name of the resource.</param>
		/// <param name="filePath">The path to the file.</param>
		public static void LoadResourceFromFile(string resourceName, string filePath)
		{
			resourceName = resourceName.ToLower().Trim();
			lock (padlock)
			{
				if (File.Exists(filePath))
				{
					string json = File.ReadAllText(filePath);
					object resource = JsonConvert.DeserializeObject(json);
					Instance.resourceCache.Add(resourceName, resource);
				}
			}
		}

		/// <summary>
		/// Loads a resource from a file using a custom load function and caches it with the specified name.
		/// </summary>
		/// <param name="resourceName">The name of the resource.</param>
		/// <param name="filePath">The path to the file.</param>
		/// <param name="loadFunc">The custom load function.</param>
		public static void LoadResourceFromFile(string resourceName, string filePath, Func<string, object> loadFunc)
		{
			resourceName = resourceName.ToLower().Trim();
			lock (padlock)
			{
				if (File.Exists(filePath))
				{
					object resource = loadFunc(filePath);
					Instance.resourceCache.Add(resourceName, resource);
				}
			}
		}

		/// <summary>
		/// Saves a resource with the specified name to a file.
		/// </summary>
		/// <param name="resourceName">The name of the resource.</param>
		/// <param name="filePath">The path to the file.</param>
		public static void SaveResourceToFile(string resourceName, string filePath)
		{
			resourceName = resourceName.ToLower().Trim();
			lock (padlock)
			{
				if (Instance.resourceCache.ContainsKey(resourceName))
				{
					object resource = Instance.resourceCache[resourceName];
					string json = JsonConvert.SerializeObject(resource);
					File.WriteAllText(filePath, json);
				}
			}
		}

		/// <summary>
		/// Unloads a resource with the specified name from the cache.
		/// </summary>
		/// <param name="resourceName">The name of the resource.</param>
		public static void UnloadResource(string resourceName)
		{
			resourceName = resourceName.ToLower().Trim();
			lock (padlock)
			{
				if (Instance.resourceCache.ContainsKey(resourceName))
				{
					Instance.resourceCache.Remove(resourceName);
				}
			}
		}

		/// <summary>
		/// Checks if the cache contains a resource with the specified name.
		/// </summary>
		/// <param name="resourceName">The key of the resource expected to be loaded</param>
		/// <returns>True if there is a loaded resource with that key, false otherwise</returns>
		public static bool HasResourceKey(string resourceName)
		{
			resourceName = resourceName.ToLower().Trim();
			lock (padlock)
			{
				return Instance.resourceCache.ContainsKey(resourceName);
			}
		}

		/// <summary>
		/// Clears the resource cache.
		/// </summary>
		public static void Clear()
		{
			lock (padlock)
			{
				Instance.resourceCache.Clear();
			}
		}
	}
}
