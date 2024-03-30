using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Raylib.Graphics
{
	public class ModelsCache
	{
		private static ModelsCache instance;
		private Dictionary<string, Model> modelDictionary;

		private ModelsCache()
		{
			modelDictionary = new Dictionary<string, Model>();
		}

		public static ModelsCache Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ModelsCache();
				}
				return instance;
			}
		}

		public Model GetModel(string modelName)
		{
			if (modelDictionary.ContainsKey(modelName))
			{
				return modelDictionary[modelName];
			}
			else
			{
				// Load the model and add it to the dictionary
				Model model = LoadModel(modelName);
				modelDictionary.Add(modelName, model);
				return model;
			}
		}

		public void CacheModel(string modelName, Model model)
		{
			if (!modelDictionary.ContainsKey(modelName))
			{
				modelDictionary.Add(modelName, model);
			}
		}

		private Model LoadModel(string modelName)
		{
			Model model = Raylib_cs.Raylib.LoadModel(modelName);
			return model;
		}
	}
}
