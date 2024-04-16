using Basalt.Raylib.Components;
using Basalt.Raylib.Utils;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Raylib.Graphics
{
	public unsafe class RaylibCache
	{
		private static RaylibCache instance;
		private Dictionary<string, Model> modelDictionary = new();
		private Dictionary<string, Shader> shaderDictionary = new();

		private List<KeyValuePair<string, ModelLoadRequest>> modelLoadQueue = new();
		private List<KeyValuePair<string, ShaderLoadRequest>> shaderLoadQueue = new();

		internal void LoadQueued()
		{
			foreach (var req in shaderLoadQueue)
			{
				Shader s = Raylib_cs.Raylib.LoadShader(req.Value.vertexShaderPath, req.Value.fragmentShaderPath);
				shaderDictionary.Add(req.Key, s);
			}
			shaderLoadQueue.Clear();

			foreach (var req in modelLoadQueue)
			{
				Model m = Raylib_cs.Raylib.LoadModel(req.Value.modelPath);
				if (!string.IsNullOrWhiteSpace(req.Value.shaderCacheKey))
				{
					for(int i = 0; i < m.MaterialCount; i++)
					{
						m.Materials[i].Shader = shaderDictionary[req.Value.shaderCacheKey];
					}
				}

					modelDictionary.Add(req.Key, m);
			}
			modelLoadQueue.Clear();
		}
		private RaylibCache()
		{
		}

		public static RaylibCache Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new RaylibCache();
				}
				return instance;
			}
		}

		public Model? GetModel(string modelName)
		{
			if (modelDictionary.ContainsKey(modelName))
			{
				return modelDictionary[modelName];
			}
			else
			{
				return null;
			}
		}

		public void CacheModel(string modelName, Model model)
		{
			if (!modelDictionary.ContainsKey(modelName))
			{
				modelDictionary.Add(modelName, model);
			}
		}

		public void UnloadAllModels()
		{
			foreach (var model in modelDictionary.Values)
			{
				Raylib_cs.Raylib.UnloadModel(model);
			}
			modelDictionary.Clear();
		}

		public void UnloadModel(string modelName)
		{
			if (modelDictionary.ContainsKey(modelName))
			{
				Raylib_cs.Raylib.UnloadModel(modelDictionary[modelName]);
				modelDictionary.Remove(modelName);
			}
		}

		public void LoadModel(string modelName, string modelPath, string shaderCacheKey = "")
		{
            if (!Raylib_cs.Raylib.IsWindowReady())
            {
				ModelLoadRequest request = new()
				{
					modelName = modelName,
					modelPath = modelPath,
					shaderCacheKey = shaderCacheKey
				};

				modelLoadQueue.Add(new KeyValuePair<string, ModelLoadRequest>(modelName, request));
				return;
                
            }
            Model model = Raylib_cs.Raylib.LoadModel(modelPath);
			modelDictionary.Add(modelName, model);
		}

		public Shader? GetShader(string shaderName)
		{
			if (shaderDictionary.ContainsKey(shaderName))
			{
				return shaderDictionary[shaderName];
			}
			else
			{
				return null;
			}
		}

		public void CacheShader(string shaderName, Shader shader)
		{
			if (!shaderDictionary.ContainsKey(shaderName))
			{
				shaderDictionary.Add(shaderName, shader);
				return;
			}
			shaderDictionary[shaderName] = shader;

		}

		public void UnloadAllShaders()
		{
			foreach (var shader in shaderDictionary.Values)
			{
				Raylib_cs.Raylib.UnloadShader(shader);
			}
			shaderDictionary.Clear();
		}

		public void UnloadShader(string shaderName)
		{
			if (shaderDictionary.ContainsKey(shaderName))
			{
				Raylib_cs.Raylib.UnloadShader(shaderDictionary[shaderName]);
				shaderDictionary.Remove(shaderName);
			}
		}

		public Shader LoadShader(string shaderName, string fragmentShaderPath, string vertexShaderPath)
		{
			if(!Raylib_cs.Raylib.IsWindowReady())
			{
				var request = new ShaderLoadRequest()
				{
					shaderName = shaderName,
					fragmentShaderPath = fragmentShaderPath,
					vertexShaderPath = vertexShaderPath
				};
				shaderLoadQueue.Add(new (shaderName, request) );
				return new Shader();
			}
			Shader shader = Raylib_cs.Raylib.LoadShader(vertexShaderPath, fragmentShaderPath);
			shaderDictionary.Add(shaderName, shader);
			return shader;
		}

		public bool HasModelKey(string key) => modelDictionary.ContainsKey(key);
		public bool HasShaderKey(string key) => shaderDictionary.ContainsKey(key);
		private struct ModelLoadRequest
		{
			public string modelName;
			public string modelPath;
			public string shaderCacheKey;
		}

		private struct ShaderLoadRequest
		{
			public string shaderName;
			public string fragmentShaderPath;
			public string vertexShaderPath;
		}
	}
}
