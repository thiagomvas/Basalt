using Newtonsoft.Json;
namespace Basalt.Common.Utils
{
	/// <summary>
	/// Represents a cache for storing and retrieving resources.
	/// </summary>
	public class ResourceCache
	{
		private static ResourceCache instance;
		private Dictionary<string, string> resourceCache;

		private ResourceCache()
		{
			resourceCache = new Dictionary<string, string>();
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
		/// Gets the resource content as a string for the specified resource name.
		/// </summary>
		/// <param name="resourceName">The name of the resource.</param>
		/// <returns>The resource content as a string.</returns>
		public static string GetResourceString(string resourceName)
		{
			return Instance.GetResource(resourceName);
		}

		/// <summary>
		/// Gets the resource content as an object of type T for the specified resource name.
		/// </summary>
		/// <typeparam name="T">The type of the resource object.</typeparam>
		/// <param name="resourceName">The name of the resource.</param>
		/// <returns>The resource content as an object of type T.</returns>
		public static T GetResourceAs<T>(string resourceName)
		{
			return Instance.GetResource<T>(resourceName);
		}

		private string GetResource(string resourceName)
		{
			if (resourceCache.ContainsKey(resourceName))
			{
				return resourceCache[resourceName];
			}
			else
			{
				string resourceContent = LoadResourceFromFile(resourceName);
				resourceCache.Add(resourceName, resourceContent);
				return resourceContent;
			}
		}

		private T GetResource<T>(string resourceName)
		{
			return JsonConvert.DeserializeAnonymousType<T>(GetResource(resourceName), default(T));
		}

		private string LoadResourceFromFile(string resourceName)
		{
			string resourceContent = File.ReadAllText(resourceName);
			return resourceContent;
		}
	}
}
