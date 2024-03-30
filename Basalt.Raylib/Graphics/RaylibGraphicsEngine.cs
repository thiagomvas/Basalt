using Basalt.Common;
using Basalt.Common.Entities;
using Basalt.Core.Common.Abstractions;
using Basalt.Core.Common.Abstractions.Sound;
using Basalt.Raylib.Components;
using Basalt.Raylib.Sound;
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
		public string PostProcessingVertexShaderPath { get; set; } = string.Empty;
		public string PostProcessingFragmentShaderPath { get; set; } = string.Empty;

		private readonly WindowInitParams config;
		private readonly ILogger? logger;
		private static RaylibGraphicsEngine instance;

		Shader PostProcessShader;
		bool ShouldRun = true;


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
			if (config.Borderless)
				SetConfigFlags(ConfigFlags.UndecoratedWindow);

			InitWindow(config.Width, config.Height, config.Title);

			SetTargetFPS(config.TargetFps);

			if (config.Fullscreen)
				ToggleFullscreen();
			if (config.VSync)
				SetConfigFlags(ConfigFlags.VSyncHint);

			if (config.MSAA4X)
				SetConfigFlags(ConfigFlags.Msaa4xHint);

			if (enablePostProcessing)
				PostProcessShader = LoadShader(PostProcessingVertexShaderPath, PostProcessingFragmentShaderPath);

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

		public void Render()
		{
			Camera3D camera = new();
			var control = Engine.Instance.EntityManager.GetEntities().FirstOrDefault(e => e is CameraController) as CameraController;
			camera = control.camera;
			control.OnStart();

			RenderTexture2D target = LoadRenderTexture(GetScreenWidth(), GetScreenHeight());

			//--------------------------------------------------------------------------------------

			bool hasSoundSystem = Engine.Instance.SoundSystem is not null;

			logger?.LogInformation("Starting raylib rendering loop...");

			Model model = LoadModelFromMesh(GenMeshCube(1.0f, 1.0f, 1.0f));
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
					int foo = 1 / (int)GetFrameTime();
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

				//----------------------------------------------------------------------------------
				// Draw
				//----------------------------------------------------------------------------------
				BeginDrawing();
				if (enablePostProcessing)
					BeginTextureMode(target);
				ClearBackground(Color.Black);

				BeginMode3D(control.camera);

				DrawModel(model, Vector3.One, 1.0f, Color.Red);


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

				DrawRectangle(600, 5, 195, 100, Fade(Color.SkyBlue, 0.5f));
				DrawRectangleLines(600, 5, 195, 100, Color.Blue);

				DrawText($"Physics Elapsed time: {Time.PhysicsDeltaTime}s - Expected: 0.016s", 15, 30, 10, Color.White);
				DrawText($"Update Elapsed time: {Time.DeltaTime}s - Expected: 0.00833s", 15, 45, 10, Color.White);
				DrawText($"Pos: {control.Transform.Position} - {control.camera.Position}", 15, 60, 10, Color.White);
				DrawText($"Rot: {control.Transform.Rotation}", 15, 75, 10, Color.White);

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

		public static T InvokeOnThread<T>(Func<T> delegateFunc)
		{
			return instance.invoke(delegateFunc);
		}

		private T invoke<T>(Func<T> delegateFunc) => delegateFunc();



		public void Shutdown()
		{
			ShouldRun = false;
			ModelsCache.Instance.UnloadAllModels();
			logger?.LogWarning("Shutting down graphics engine...");
		}
	}
}
