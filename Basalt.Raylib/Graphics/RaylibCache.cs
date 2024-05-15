using Raylib_cs;

namespace Basalt.Raylib.Graphics
{
	/// <summary>
	/// Represents a cache for Raylib models and shaders.
	/// </summary>
	public unsafe class RaylibCache
	{
		/// <summary>
		/// The singleton instance of the RaylibCache class.
		/// </summary>
		private static RaylibCache instance;

		/// <summary>
		/// The dictionary that stores the cached models.
		/// </summary>
		private Dictionary<string, Model> modelDictionary = new();

		/// <summary>
		/// The dictionary that stores the cached shaders.
		/// </summary>
		private Dictionary<string, Shader> shaderDictionary = new();

		/// <summary>
		/// The queue that stores the model load requests.
		/// </summary>
		private List<KeyValuePair<string, ModelLoadRequest>> modelLoadQueue = new();

		/// <summary>
		/// The queue that stores the shader load requests.
		/// </summary>
		private List<KeyValuePair<string, ShaderLoadRequest>> shaderLoadQueue = new();

		/// <summary>
		/// The lock object used for thread synchronization when accessing the model dictionary.
		/// </summary>
		private object modelLock = new object();

		/// <summary>
		/// The lock object used for thread synchronization when accessing the shader dictionary.
		/// </summary>
		private object shaderLock = new object();

		/// <summary>
		/// Loads the models and shaders from the load queues.
		/// </summary>
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

		/// <summary>
		/// Private constructor to prevent direct instantiation of the RaylibCache class.
		/// </summary>
		private RaylibCache()
		{
		}

		/// <summary>
		/// Gets the singleton instance of the RaylibCache class.
		/// </summary>
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

		/// <summary>
		/// Gets the cached model with the specified name.
		/// </summary>
		/// <param name="modelName">The name of the model.</param>
		/// <returns>The cached model, or null if the model is not found.</returns>
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

		/// <summary>
		/// Caches the specified model with the given name.
		/// </summary>
		/// <param name="modelName">The name of the model.</param>
		/// <param name="model">The model to cache.</param>
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

		/// <summary>
		/// Unloads all the cached models.
		/// </summary>
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

		/// <summary>
		/// Unloads the cached model with the specified name.
		/// </summary>
		/// <param name="modelName">The name of the model to unload.</param>
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

		/// <summary>
		/// Loads a model with the specified name, model path, and optional shader cache key.
		/// </summary>
		/// <param name="modelName">The name of the model.</param>
		/// <param name="modelPath">The path to the model file.</param>
		/// <param name="shaderCacheKey">The optional shader cache key.</param>
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

		/// <summary>
		/// Gets the cached shader with the specified name.
		/// </summary>
		/// <param name="shaderName">The name of the shader.</param>
		/// <returns>The cached shader, or null if the shader is not found.</returns>
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

		/// <summary>
		/// Caches the specified shader with the given name.
		/// </summary>
		/// <param name="shaderName">The name of the shader.</param>
		/// <param name="shader">The shader to cache.</param>
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

		/// <summary>
		/// Unloads all the cached shaders.
		/// </summary>
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

		/// <summary>
		/// Unloads the cached shader with the specified name.
		/// </summary>
		/// <param name="shaderName">The name of the shader to unload.</param>
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

		/// <summary>
		/// Loads a shader with the specified name, fragment shader path, and vertex shader path.
		/// </summary>
		/// <param name="shaderName">The name of the shader.</param>
		/// <param name="fragmentShaderPath">The path to the fragment shader file.</param>
		/// <param name="vertexShaderPath">The path to the vertex shader file.</param>
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

		/// <summary>
		/// Checks if the model dictionary contains the specified key.
		/// </summary>
		/// <param name="key">The key to check.</param>
		/// <returns>True if the model dictionary contains the key, false otherwise.</returns>
		public bool HasModelKey(string key)
		{
			lock (modelLock)
			{
				return modelDictionary.ContainsKey(key);
			}
		}

		/// <summary>
		/// Checks if the shader dictionary contains the specified key.
		/// </summary>
		/// <param name="key">The key to check.</param>
		/// <returns>True if the shader dictionary contains the key, false otherwise.</returns>
		public bool HasShaderKey(string key)
		{
			lock (shaderLock)
			{
				return shaderDictionary.ContainsKey(key);
			}
		}

		/// <summary>
		/// Represents a model load request.
		/// </summary>
		private struct ModelLoadRequest
		{
			public string modelName;
			public string modelPath;
			public string shaderCacheKey;
		}

		/// <summary>
		/// Represents a shader load request.
		/// </summary>
		private struct ShaderLoadRequest
		{
			public string shaderName;
			public string fragmentShaderPath;
			public string vertexShaderPath;
		}
	}
}
