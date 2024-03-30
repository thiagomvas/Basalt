using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Common.Utils
{
	public class ResourceCache
	{
		private static ResourceCache instance;
		private Dictionary<string, string> resourceCache;

		private ResourceCache()
		{
			resourceCache = new Dictionary<string, string>();
		}

		private static ResourceCache Instance
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

		public static string GetResourceString(string resourceName)
		{
			return Instance.GetResource(resourceName);
		}

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
