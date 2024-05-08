using Basalt.Common;
using Basalt.Common.Entities;
using Basalt.Core.Common.Abstractions;
using Basalt.Core.Common.Abstractions.Sound;
using Basalt.Raylib.Components;
using Basalt.Raylib.Sound;
using Basalt.Raylib.Utils;
using Basalt.Types;
using Raylib_cs;
using System.Numerics;
using System.Runtime.InteropServices;
using static Raylib_cs.Raylib;

namespace Basalt.Raylib.Graphics
{
	public class RaylibGraphicsEngine : IGraphicsEngine
	{

		public const int MaxColumns = 20;
		private bool enablePostProcessing = false;

		public string LightingShaderCacheKey { get; set; } = string.Empty;
		public string PostProcessingShaderCacheKey { get; set; } = string.Empty;

		private readonly WindowInitParams config;
		private readonly ILogger? logger;
		internal static RaylibGraphicsEngine instance;

		Shader PostProcessShader, LightShader;
		bool ShouldRun = true;
		internal List<LightSource> sources = new();
		List<LightSource> sourcesToInit = new();

		bool useLighting = false;

		public RaylibGraphicsEngine(WindowInitParams initConfig, ILogger? logger = null)
		{
			config = initConfig;
			this.logger = logger;
		}

		public unsafe void Initialize()
		{
			enablePostProcessing = config.PostProcessing;
			instance = this;
			SetTraceLogCallback(&LogCustom);
			SetTraceLogLevel(TraceLogLevel.All);
			if (config.Borderless)
				SetConfigFlags(ConfigFlags.UndecoratedWindow);


			if (config.MSAA4X)
				SetConfigFlags(ConfigFlags.Msaa4xHint);

			InitWindow(config.Width, config.Height, config.Title);

			RaylibCache.Instance.LoadQueued();

			if (RaylibCache.Instance.HasShaderKey(LightingShaderCacheKey))
			{
				LightShader = RaylibCache.Instance.GetShader(LightingShaderCacheKey)!.Value;
				useLighting = true;
			}
			RaylibCache.Instance.CacheShader("lighting", LightShader);

			Engine.Instance.count.Signal();


			SetTargetFPS(config.TargetFps);

			if (config.Fullscreen)
				ToggleFullscreen();
			if (config.VSync)
				SetConfigFlags(ConfigFlags.VSyncHint);

			if (enablePostProcessing)
				PostProcessShader = RaylibCache.Instance.GetShader(PostProcessingShaderCacheKey)!.Value;

			DisableCursor();

			logger?.LogInformation("Graphics Engine Initialized");
			try
			{
				Render();
			}
			catch
			{
				ShouldRun = false;
				logger?.LogError($"Graphics Engine stopped running due to an exception.");
				throw;
			}
			finally
			{
				Engine.Instance.Shutdown();
			}
		}

		public unsafe void Render()
		{
			Camera3D camera = new();
			var control = Engine.Instance.EntityManager.GetEntities().FirstOrDefault(e => e is CameraController) as CameraController;
			camera = control.camera;
			control.OnStart();

			RenderTexture2D target = LoadRenderTexture(GetScreenWidth(), GetScreenHeight());

			//--------------------------------------------------------------------------------------



			bool hasSoundSystem = Engine.Instance.SoundSystem is not null;

			logger?.LogInformation("Starting raylib rendering loop...");

			// Get some required shader loactions
			if (useLighting)
			{
				LightShader.Locs[(int)ShaderLocationIndex.VectorView] = GetShaderLocation(LightShader, "viewPos");

				// ambient light level
				int ambientLoc = GetShaderLocation(LightShader, "ambient");
				float[] ambient = new[] { 0.1f, 0.1f, 0.1f, 1.0f };
				SetShaderValue(LightShader, ambientLoc, ambient, ShaderUniformDataType.Vec4);
			}



			Image image = LoadImage("perlin_noise.png");
			Image imageinvert = LoadImage("perlin_noise_colored.png");
			var texture = LoadTextureFromImage(image);
			var textureinvert = LoadTextureFromImage(imageinvert);

			Mesh mesh = GenMeshHeightmap(image, new Vector3(256, 32, 256));
			Model model = LoadModelFromMesh(mesh);

			model.Materials[0].Shader = LightShader;

			SetMaterialTexture(ref model, 0, MaterialMapIndex.Albedo, ref textureinvert);

			RaylibCache.Instance.CacheModel("heightmap", model);

			Entity heightmap = new();
			heightmap.AddComponent(new ModelRenderer(heightmap) { ModelCacheKey = "heightmap" });
			heightmap.Transform.Position = new Vector3(-128, -16, -128);
			Engine.CreateEntity(heightmap);

			UnloadImage(image);
			UnloadImage(imageinvert);


			// Main game loop
			while (ShouldRun)
			{
				control.OnUpdate();
				if (hasSoundSystem && Engine.Instance.SoundSystem!.IsMusicPlaying())
				{
					UpdateMusicStream((Music)Engine.Instance.SoundSystem.GetMusicPlaying()!);
				}
				Time.DeltaTime = GetFrameTime();

				if (IsKeyPressed(KeyboardKey.G))
				{
					Engine.Instance.SoundSystem?.PlayAudio("testaudio.mp3", AudioType.SoundEffect);
				}

				if (IsKeyPressed(KeyboardKey.H))
				{
					Engine.Instance.SoundSystem?.PlayAudio("testsong.mp3", AudioType.Music);
				}

				if (IsKeyPressed(KeyboardKey.J))
				{
					Engine.Instance.SoundSystem?.PauseAudio(AudioType.Music);
				}

				if (IsKeyPressed(KeyboardKey.K))
				{
					Engine.Instance.SoundSystem?.ResumeAudio(AudioType.Music);
				}

				if (IsKeyPressed(KeyboardKey.L))
				{
					Engine.Instance.SoundSystem?.StopAudio(AudioType.Music);
				}


				if (IsKeyPressed(KeyboardKey.Escape))
				{
					Engine.Instance.Shutdown();
					ShouldRun = false;
				}
				// Update
				//----------------------------------------------------------------------------------
				Engine.Instance.EventBus?.NotifyUpdate();
				Engine.Instance.InputSystem?.Update();



				// MANUALLY UPDATING LIGHTS HERE SO IT ACTUALLY WORKS. DOES NOT WORK IF UPDATES OUTSIDE THIS THREAD
				// TO-DO: FIND A WAY TO REMOVE THIS FROM HERE.
				if (sourcesToInit.Count > 0)
				{
					foreach (var source in sourcesToInit)
					{
						source.Setup();
					}
					sourcesToInit.Clear();
				}

				if (useLighting)
				{

					foreach (var source in sources)
						Rlights.UpdateLightValues(LightShader, source.Source);

					SetShaderValue(LightShader, LightShader.Locs[(int)ShaderLocationIndex.VectorView], control.Transform.Position, ShaderUniformDataType.Vec3);
				}

				//----------------------------------------------------------------------------------
				// Draw
				//----------------------------------------------------------------------------------
				BeginDrawing();
				if (enablePostProcessing)
					BeginTextureMode(target);
				ClearBackground(Color.Black);

				BeginMode3D(control.camera);

#if DEBUG
				DrawGrid(100, 1.0f);
#endif

				Engine.Instance.EventBus?.NotifyRender();


				EndMode3D();

				if (enablePostProcessing)
				{
					EndTextureMode();

					BeginShaderMode(PostProcessShader);
					DrawTextureRec(target.Texture, new Rectangle(0, 0, target.Texture.Width, -target.Texture.Height), new Vector2(0, 0), Color.White);
					EndShaderMode();

				}


				// Draw info boxes
				DrawRectangle(5, 5, 330, 100, ColorAlpha(Color.SkyBlue, 0.5f));
				DrawRectangleLines(10, 10, 330, 100, Color.Blue);

				DrawText($"Physics Elapsed time: {Time.PhysicsDeltaTime}s - Expected: 0.016s", 15, 30, 10, Color.White);
				DrawText($"Update Elapsed time: {Time.DeltaTime}s - Expected: 0.00833s", 15, 45, 10, Color.White);
				DrawFPS(15, 105);

				EndDrawing();
				//----------------------------------------------------------------------------------
			}
			CloseWindow();
			logger?.LogWarning("Graphics Engine stopped running gracefully.");

		}

		[UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
		private static unsafe void LogCustom(int logLevel, sbyte* text, sbyte* args)
		{
			var message = Logging.GetLogMessage(new IntPtr(text), new IntPtr(args));
			switch ((TraceLogLevel)logLevel)
			{
				case TraceLogLevel.All:
					instance.logger?.LogDebug(message);
					break;
				case TraceLogLevel.Trace:
					instance.logger?.LogDebug(message);
					break;
				case TraceLogLevel.Debug:
					instance.logger?.LogDebug(message);
					break;
				case TraceLogLevel.Info:
					instance.logger?.LogInformation(message);
					break;
				case TraceLogLevel.Warning:
					instance.logger?.LogWarning(message);
					break;
				case TraceLogLevel.Error:
					instance.logger?.LogError(message);
					break;
				case TraceLogLevel.Fatal:
					instance.logger?.LogFatal(message);
					break;
				default:
					break;
			}

		}

		internal static void instantiateLight(LightSource source)
		{
			instance.sourcesToInit.Add(source);
			instance.sources.Add(source);
		}

		internal static void destroyLight(LightSource source) => instance.sources.Remove(source);

		public static T InvokeOnThread<T>(Func<T> delegateFunc)
		{
			return instance.invoke(delegateFunc);
		}

		private T invoke<T>(Func<T> delegateFunc) => delegateFunc();



		public void Shutdown()
		{
			ShouldRun = false;
			RaylibCache.Instance.UnloadAllModels();
			logger?.LogWarning("Shutting down graphics engine...");
		}
	}
}
