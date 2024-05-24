using Basalt.Common;
using Basalt.Common.Entities;
using Basalt.Common.Utils;
using Basalt.Core.Common.Abstractions.Engine;
using Basalt.Core.Common.Abstractions.Sound;
using Basalt.Raylib.Components;
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
		private ILogger? logger;
		internal static RaylibGraphicsEngine instance = new(new());

		private EntityManager entityManager;
		private ISoundSystem? soundSystem;
		private IEventBus eventBus;

		Shader PostProcessShader, LightShader;
		bool ShouldRun = true;
		internal List<LightSource> sources = new();
		List<LightSource> sourcesToInit = new();

		bool useLighting = false;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public RaylibGraphicsEngine(WindowInitParams initConfig, ILogger? logger = null)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		{
			config = initConfig;
			this.logger = logger;
		}

		public unsafe void Initialize()
		{
			soundSystem = Engine.Instance.GetEngineComponent<ISoundSystem>();
			eventBus = Engine.Instance.GetEngineComponent<IEventBus>()!;
			entityManager = Engine.Instance.EntityManager;
			logger = Engine.Instance.Logger;



			enablePostProcessing = config.PostProcessing;
			instance = this;
			SetTraceLogCallback(&LogToLogger);
			SetTraceLogLevel(TraceLogLevel.All);
			if (config.Borderless)
				SetConfigFlags(ConfigFlags.UndecoratedWindow);


			if (config.MSAA4X)
				SetConfigFlags(ConfigFlags.Msaa4xHint);

			InitWindow(config.Width, config.Height, config.Title);

			ResourceCache.Instance.LoadQueued();

			if (ResourceCache.TryGetResource(LightingShaderCacheKey, out LightShader))
			{
				useLighting = true;
				ResourceCache.CacheResource("basalt.shaders.defaultlight", LightShader);
			}
			ResourceCache.Instance.LoadRaylibPrimitives();


			SetTargetFPS(config.TargetFps);

			if (config.Fullscreen)
				ToggleFullscreen();
			if (config.VSync)
				SetConfigFlags(ConfigFlags.VSyncHint);

			if (enablePostProcessing)
				PostProcessShader = ResourceCache.Instance.GetShader(PostProcessingShaderCacheKey)!.Value;

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
			var control = entityManager.GetEntities().FirstOrDefault(e => e.GetComponent<CameraController>() != null)?.GetComponent<CameraController>() ?? null;
			if (control == null)
			{
				throw new NullReferenceException("No camera controller found in the scene.");
			}

			camera = control!.camera;
			control.OnStart();

			RenderTexture2D target = LoadRenderTexture(GetScreenWidth(), GetScreenHeight());

			//--------------------------------------------------------------------------------------



			bool hasSoundSystem = soundSystem is not null;

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

			// Main game loop
			while (ShouldRun)
			{
				control.OnUpdate();
				Time.DeltaTime = GetFrameTime();



				if (IsKeyPressed(KeyboardKey.Escape))
				{
					Engine.Instance.Shutdown();
					ShouldRun = false;
				}
				// Update
				//----------------------------------------------------------------------------------
				eventBus?.TriggerEvent(BasaltConstants.UpdateEventKey);



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

					SetShaderValue(LightShader, LightShader.Locs[(int)ShaderLocationIndex.VectorView], control.Entity.Transform.Position, ShaderUniformDataType.Vec3);
				}

				//----------------------------------------------------------------------------------
				// Draw
				//----------------------------------------------------------------------------------
				BeginDrawing();
				if (enablePostProcessing)
					BeginTextureMode(target);
				ClearBackground(Color.Black);

				BeginMode3D(control.camera);

				eventBus?.TriggerEvent(BasaltConstants.RenderEventKey);


				EndMode3D();

				if (enablePostProcessing)
				{
					EndTextureMode();

					BeginShaderMode(PostProcessShader);
					DrawTextureRec(target.Texture, new Rectangle(0, 0, target.Texture.Width, -target.Texture.Height), new Vector2(0, 0), Color.White);
					EndShaderMode();

				}

				EndDrawing();
				//----------------------------------------------------------------------------------
			}
			CloseWindow();
			logger?.LogWarning("Graphics Engine stopped running gracefully.");

		}

		[UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
		private static unsafe void LogToLogger(int logLevel, sbyte* text, sbyte* args)
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
			ResourceCache.Instance.UnloadRaylib();
			logger?.LogWarning("Shutting down graphics engine...");
		}
	}
}
