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

		private object modelLock = new object();
		private object shaderLock = new object();

		internal void LoadQueued()
		{
			lock (shaderLock)
			{
				foreach (var req in shaderLoadQueue)
				{
					Shader s = Raylib_cs.Raylib.LoadShader(req.Value.vertexShaderPath, req.Value.fragmentShaderPath);
					shaderDictionary.Add(req.Key, s);
				}
				shaderLoadQueue.Clear();
			}

			lock (modelLock)
			{
				foreach (var req in modelLoadQueue)
				{
					Model m = Raylib_cs.Raylib.LoadModel(req.Value.modelPath);
					if (!string.IsNullOrWhiteSpace(req.Value.shaderCacheKey))
					{
						for (int i = 0; i < m.MaterialCount; i++)
						{
							m.Materials[i].Shader = shaderDictionary[req.Value.shaderCacheKey];
						}
					}

					modelDictionary.Add(req.Key, m);
				}
				modelLoadQueue.Clear();
			}
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
			lock (modelLock)
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
		}

		public void CacheModel(string modelName, Model model)
		{
			lock (modelLock)
			{
				if (!modelDictionary.ContainsKey(modelName))
				{
					modelDictionary.Add(modelName, model);
				}
			}
		}

		public void UnloadAllModels()
		{
			lock (modelLock)
			{
				foreach (var model in modelDictionary.Values)
				{
					Raylib_cs.Raylib.UnloadModel(model);
				}
				modelDictionary.Clear();
			}
		}

		public void UnloadModel(string modelName)
		{
			lock (modelLock)
			{
				if (modelDictionary.ContainsKey(modelName))
				{
					Raylib_cs.Raylib.UnloadModel(modelDictionary[modelName]);
					modelDictionary.Remove(modelName);
				}
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

				lock (modelLock)
				{
					modelLoadQueue.Add(new KeyValuePair<string, ModelLoadRequest>(modelName, request));
					return;
				}
			}

			Model model = Raylib_cs.Raylib.LoadModel(modelPath);

			lock (modelLock)
			{
				modelDictionary.Add(modelName, model);
			}
		}

		public Shader? GetShader(string shaderName)
		{
			lock (shaderLock)
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
		}

		public void CacheShader(string shaderName, Shader shader)
		{
			lock (shaderLock)
			{
				if (!shaderDictionary.ContainsKey(shaderName))
				{
					shaderDictionary.Add(shaderName, shader);
					return;
				}
				shaderDictionary[shaderName] = shader;
			}
		}

		public void UnloadAllShaders()
		{
			lock (shaderLock)
			{
				foreach (var shader in shaderDictionary.Values)
				{
					Raylib_cs.Raylib.UnloadShader(shader);
				}
				shaderDictionary.Clear();
			}
		}

		public void UnloadShader(string shaderName)
		{
			lock (shaderLock)
			{
				if (shaderDictionary.ContainsKey(shaderName))
				{
					Raylib_cs.Raylib.UnloadShader(shaderDictionary[shaderName]);
					shaderDictionary.Remove(shaderName);
				}
			}
		}

		public void LoadShader(string shaderName, string fragmentShaderPath, string vertexShaderPath)
		{
			if (!Raylib_cs.Raylib.IsWindowReady())
			{
				var request = new ShaderLoadRequest()
				{
					shaderName = shaderName,
					fragmentShaderPath = fragmentShaderPath,
					vertexShaderPath = vertexShaderPath
				};
				lock (shaderLock)
				{
					shaderLoadQueue.Add(new(shaderName, request));
					return;
				}
			}

			Shader shader = Raylib_cs.Raylib.LoadShader(vertexShaderPath, fragmentShaderPath);

			lock (shaderLock)
			{
				shaderDictionary.Add(shaderName, shader);
			}
		}

		public bool HasModelKey(string key)
		{
			lock (modelLock)
			{
				return modelDictionary.ContainsKey(key);
			}
		}

		public bool HasShaderKey(string key)
		{
			lock (shaderLock)
			{
				return shaderDictionary.ContainsKey(key);
			}
		}

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
