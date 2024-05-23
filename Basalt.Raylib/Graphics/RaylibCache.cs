using Basalt.Common.Utils;
using Raylib_cs;
using System.Net.NetworkInformation;

namespace Basalt.Raylib.Graphics
{
	public unsafe static class ResourceCacheExtensions
	{

		/// <summary>
		/// The lock object used for thread synchronization when accessing the model dictionary.
		/// </summary>
		private static object modelLock = new object();

		/// <summary>
		/// The lock object used for thread synchronization when accessing the shader dictionary.
		/// </summary>
		private static object shaderLock = new object();
		/// <summary>
		/// The queue that stores the model load requests.
		/// </summary>
		private static List<KeyValuePair<string, ModelLoadRequest>> modelLoadQueue = new();

		/// <summary>
		/// The queue that stores the shader load requests.
		/// </summary>
		private static List<KeyValuePair<string, ShaderLoadRequest>> shaderLoadQueue = new();

		private static List<string> modelKeys = new();
		private static List<string> shaderKeys = new();

		/// <summary>
		/// Loads the models and shaders from the load queues.
		/// </summary>
		internal static void LoadQueued(this ResourceCache cache)
		{
			lock (shaderLock)
			{
				foreach (var req in shaderLoadQueue)
				{
					loadShader(req.Value);
				}
				shaderLoadQueue.Clear();
			}

			lock (modelLock)
			{
				foreach (var req in modelLoadQueue)
				{
					loadModel(req.Value);
				}
				modelLoadQueue.Clear();
			}
		}

		/// <summary>
		/// Loads a model with the specified name, model path, and optional shader cache key.
		/// </summary>
		/// <param name="modelName">The name of the model.</param>
		/// <param name="modelPath">The path to the model file.</param>
		/// <param name="shaderCacheKey">The optional shader cache key.</param>
		public static void LoadModel(this ResourceCache cache, string modelName, string modelPath, string shaderCacheKey = "")
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
				loadModel(new(modelName, modelPath, shaderCacheKey));
			}
		}
		/// <summary>
		/// Loads a shader from the specified fragment and vertex shader paths and caches it.
		/// </summary>
		/// <param name="cache">The resource cache instance.</param>
		/// <param name="shaderName">The name of the shader to load.</param>
		/// <param name="fragmentShaderPath">The path to the fragment shader file.</param>
		/// <param name="vertexShaderPath">The path to the vertex shader file.</param>
		public static void LoadShader(this ResourceCache cache, string shaderName, string fragmentShaderPath, string vertexShaderPath)
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
				ResourceCache.CacheResource(shaderName, shader);
			}
		}

		/// <summary>
		/// Retrieves a cached model by name.
		/// </summary>
		/// <param name="cache">The resource cache instance.</param>
		/// <param name="modelName">The name of the model to retrieve.</param>
		/// <returns>The cached model if found; otherwise, null.</returns>
		public static Model? GetModel(this ResourceCache cache, string modelName)
		{
			lock (modelLock)
			{
				if (ResourceCache.GetResource<Model>(modelName) is Model model)
				{
					return model;
				}
				else
				{
					return null;
				}
			}
		}

		/// <summary>
		/// Retrieves a cached shader by name.
		/// </summary>
		/// <param name="cache">The resource cache instance.</param>
		/// <param name="shaderName">The name of the shader to retrieve.</param>
		/// <returns>The cached shader if found; otherwise, null.</returns>
		public static Shader? GetShader(this ResourceCache cache, string shaderName)
		{
			lock (shaderLock)
			{
				if (ResourceCache.GetResource<Shader>(shaderName) is Shader shader)
				{
					return shader;
				}
				else
				{
					return null;
				}
			}
		}

		/// <summary>
		/// Caches a model with the specified name.
		/// </summary>
		/// <param name="cache">The resource cache instance.</param>
		/// <param name="modelName">The name of the model to cache.</param>
		/// <param name="model">The model to cache.</param>
		public static void CacheModel(this ResourceCache cache, string modelName, Model model)
		{
			lock (modelLock)
			{
				ResourceCache.CacheResource(modelName, model);
			}
		}

		/// <summary>
		/// Caches a shader with the specified name.
		/// </summary>
		/// <param name="cache">The resource cache instance.</param>
		/// <param name="shaderName">The name of the shader to cache.</param>
		/// <param name="shader">The shader to cache.</param>
		public static void CacheShader(this ResourceCache cache, string shaderName, Shader shader)
		{
			lock (shaderLock)
			{
				ResourceCache.CacheResource(shaderName, shader);
			}
		}

		/// <summary>
		/// Loads a model based on the specified request.
		/// </summary>
		/// <param name="req">The model load request containing model details.</param>
		private static void loadModel(ModelLoadRequest req)
		{
			Model m = Raylib_cs.Raylib.LoadModel(req.modelPath);
			if (!string.IsNullOrWhiteSpace(req.shaderCacheKey))
			{
				for (int i = 0; i < m.MaterialCount; i++)
				{
					m.Materials[i].Shader = ResourceCache.GetResource<Shader>(req.shaderCacheKey);
				}
			}

			ResourceCache.CacheResource(req.modelName, m);

			modelKeys.Add(req.modelName);
		}

		/// <summary>
		/// Loads a shader based on the specified request.
		/// </summary>
		/// <param name="req">The shader load request containing shader details.</param>
		private static void loadShader(ShaderLoadRequest req)
		{
			Shader s = Raylib_cs.Raylib.LoadShader(req.vertexShaderPath, req.fragmentShaderPath);
			ResourceCache.CacheResource(req.shaderName, s);

			shaderKeys.Add(req.shaderName);
		}

		/// <summary>
		/// Unloads all cached Raylib models and shaders.
		/// </summary>
		/// <param name="cache">The resource cache instance.</param>
		public static void UnloadRaylib(this ResourceCache cache)
		{
			lock (modelLock)
			{
				foreach (var key in modelKeys)
				{
					Raylib_cs.Raylib.UnloadModel(ResourceCache.GetResource<Model>(key));
				}
				modelKeys.Clear();
			}

			lock (shaderLock)
			{
				foreach (var key in shaderKeys)
				{
					Raylib_cs.Raylib.UnloadShader(ResourceCache.GetResource<Shader>(key));
				}
				shaderKeys.Clear();
			}
		}

		internal static void LoadRaylibPrimitives(this ResourceCache cache)
		{
			// Models
			Model cube = Raylib_cs.Raylib.LoadModelFromMesh(Raylib_cs.Raylib.GenMeshCube(1, 1, 1));
			Model sphere = Raylib_cs.Raylib.LoadModelFromMesh(Raylib_cs.Raylib.GenMeshSphere(1, 16, 16));
			Model plane = Raylib_cs.Raylib.LoadModelFromMesh(Raylib_cs.Raylib.GenMeshPlane(1, 1, 1, 1));
			Model cylinder = Raylib_cs.Raylib.LoadModelFromMesh(Raylib_cs.Raylib.GenMeshCylinder(1, 1, 16));
			Model torus = Raylib_cs.Raylib.LoadModelFromMesh(Raylib_cs.Raylib.GenMeshTorus(0.25f, 1, 16, 16));
			Model knot = Raylib_cs.Raylib.LoadModelFromMesh(Raylib_cs.Raylib.GenMeshKnot(1, 1, 16, 128));

			// Add shader if found
			if(ResourceCache.TryGetResource("basalt.shaders.defaultlight", out Shader shader))
			{
				cube.Materials[0].Shader = shader;
				sphere.Materials[0].Shader = shader;
				plane.Materials[0].Shader = shader;
				cylinder.Materials[0].Shader = shader;
				torus.Materials[0].Shader = shader;
				knot.Materials[0].Shader = shader;
			}

			ResourceCache.CacheResource("cube", cube);
			ResourceCache.CacheResource("sphere", sphere);
			ResourceCache.CacheResource("plane", plane);
			ResourceCache.CacheResource("cylinder", cylinder);
			ResourceCache.CacheResource("torus", torus);
			ResourceCache.CacheResource("knot", knot);
		}


		/// <summary>
		/// Represents a model load request.
		/// </summary>
		private struct ModelLoadRequest
		{
			public string modelName;
			public string modelPath;
			public string shaderCacheKey;

			public ModelLoadRequest(string modelName, string modelPath, string shaderCacheKey)
			{
				this.modelName = modelName;
				this.modelPath = modelPath;
				this.shaderCacheKey = shaderCacheKey;
			}
		}

		/// <summary>
		/// Represents a shader load request.
		/// </summary>
		private struct ShaderLoadRequest
		{
			public string shaderName;
			public string fragmentShaderPath;
			public string vertexShaderPath;

			public ShaderLoadRequest(string shaderName, string fragmentShaderPath, string vertexShaderPath)
			{
				this.shaderName = shaderName;
				this.fragmentShaderPath = fragmentShaderPath;
				this.vertexShaderPath = vertexShaderPath;
			}
		}
	}
}
