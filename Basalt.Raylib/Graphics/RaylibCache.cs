using Basalt.Common.Utils;
using Raylib_cs;

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

		private static object textureLock = new object();
		private static object audioLock = new object();
		/// <summary>
		/// The queue that stores the model load requests.
		/// </summary>
		private static List<KeyValuePair<string, ModelLoadRequest>> modelLoadQueue = new();

		/// <summary>
		/// The queue that stores the shader load requests.
		/// </summary>
		private static List<KeyValuePair<string, ShaderLoadRequest>> shaderLoadQueue = new();

		private static List<KeyValuePair<string, TextureLoadRequest>> textureLoadQueue = new();
		private static List<KeyValuePair<string, AudioLoadRequest>> audioLoadQueue = new();

		private static List<string> modelKeys = new();
		private static List<string> shaderKeys = new();
		private static List<string> textureKeys = new();
		private static List<string> audioKeys = new();

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

			lock (textureLock)
			{
				foreach (var req in textureLoadQueue)
				{
					loadTexture(req.Value);
				}
				textureLoadQueue.Clear();
			}

			lock (audioLock)
			{
				foreach (var req in audioLoadQueue)
				{
					loadAudio(req.Value);
				}
				audioLoadQueue.Clear();
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
			var request = new ShaderLoadRequest()
			{
				shaderName = shaderName,
				fragmentShaderPath = fragmentShaderPath,
				vertexShaderPath = vertexShaderPath
			};
			if (!Raylib_cs.Raylib.IsWindowReady())
			{
				lock (shaderLock)
				{
					shaderLoadQueue.Add(new(shaderName, request));
					return;
				}
			}
			lock (shaderLock)
			{
				loadShader(request);
			}
		}
		public static void LoadAudio(this ResourceCache cache, string audioName, string audioPath, AudioLoadRequest.Type loadType)
		{
			if (!Raylib_cs.Raylib.IsWindowReady())
			{
				var request = new AudioLoadRequest()
				{
					audioName = audioName,
					audioPath = audioPath,
					loadType = loadType
				};
				lock (shaderLock)
				{
					audioLoadQueue.Add(new(audioName, request));
					return;
				}
			}
			lock (audioLock)
			{
				loadAudio(new(audioName, audioPath, loadType));
			}
		}

		public static void LoadTexture(this ResourceCache cache, string textureName, string texturePath)
		{
			if (!Raylib_cs.Raylib.IsWindowReady())
			{
				var request = new TextureLoadRequest()
				{
					textureName = textureName,
					texturePath = texturePath
				};
				lock (textureLock)
				{
					textureLoadQueue.Add(new(textureName, request));
					return;
				}
			}

			lock (textureLock)
			{
				loadTexture(new(textureName, texturePath));
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

		public static Texture2D? GetTexture(this ResourceCache cache, string textureName)
		{
			lock (textureLock)
			{
				if (ResourceCache.GetResource<Texture2D>(textureName) is Texture2D texture)
				{
					return texture;
				}
				else
				{
					return null;
				}
			}
		}

		public static Music? GetMusic(this ResourceCache cache, string musicName)
		{
			if (ResourceCache.GetResource<Music>(musicName) is Music music)
			{
				return music;
			}
			else
			{
				return null;
			}
		}

		public static Raylib_cs.Sound? GetSound(this ResourceCache cache, string soundName)
		{
			if (ResourceCache.GetResource<Raylib_cs.Sound>(soundName) is Raylib_cs.Sound sound)
			{
				return sound;
			}
			else
			{
				return null;
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

		public static void CacheTexture(this ResourceCache cache, string textureName, Texture2D texture)
		{
			lock (textureLock)
			{
				ResourceCache.CacheResource(textureName, texture);
			}
		}

		public static void CacheMusic(this ResourceCache cache, string musicName, Music music)
		{
			ResourceCache.CacheResource(musicName, music);
		}

		public static void CacheSound(this ResourceCache cache, string soundName, Raylib_cs.Sound sound)
		{
			ResourceCache.CacheResource(soundName, sound);
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

		private static void loadTexture(TextureLoadRequest req)
		{
			Texture2D texture = Raylib_cs.Raylib.LoadTexture(req.texturePath);
			ResourceCache.CacheResource(req.textureName, texture);
			textureKeys.Add(req.textureName);
		}

		private static void loadAudio(AudioLoadRequest req)
		{
			switch (req.loadType)
			{
				case AudioLoadRequest.Type.Music:
					Music music = Raylib_cs.Raylib.LoadMusicStream(req.audioPath);
					ResourceCache.CacheResource(req.audioName, music);
					break;
				case AudioLoadRequest.Type.Sound:
					Raylib_cs.Sound sound = Raylib_cs.Raylib.LoadSound(req.audioPath);
					ResourceCache.CacheResource(req.audioName, sound);
					break;
			}
			Engine.Instance.Logger.LogInformation($"Loaded audio: {req.audioName}");
			audioKeys.Add(req.audioName);
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

			lock (textureLock)
			{
				foreach (var key in textureKeys)
				{
					Raylib_cs.Raylib.UnloadTexture(ResourceCache.GetResource<Texture2D>(key));
				}
				textureKeys.Clear();
			}

			lock (audioLock)
			{
				foreach (var key in audioKeys)
				{
					if (ResourceCache.TryGetResource(key, out Music m))
					{
						Raylib_cs.Raylib.UnloadMusicStream(m);
					}
					else if (ResourceCache.TryGetResource(key, out Raylib_cs.Sound s))
					{
						Raylib_cs.Raylib.UnloadSound(s);
					}
				}
				audioKeys.Clear();
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
			if (ResourceCache.TryGetResource("basalt.shaders.defaultlight", out Shader shader))
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

		private struct TextureLoadRequest
		{
			public string textureName;
			public string texturePath;

			public TextureLoadRequest(string textureName, string texturePath)
			{
				this.textureName = textureName;
				this.texturePath = texturePath;
			}
		}

		public struct AudioLoadRequest
		{
			public string audioName;
			public string audioPath;
			public Type loadType;
			public enum Type
			{
				Music,
				Sound,
			}

			public AudioLoadRequest(string audioName, string audioPath, Type loadType)
			{
				this.audioName = audioName;
				this.audioPath = audioPath;
				this.loadType = loadType;
			}
		}
	}
}
