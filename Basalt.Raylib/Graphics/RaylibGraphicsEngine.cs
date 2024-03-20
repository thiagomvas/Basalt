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

		private readonly WindowInitParams config;
		private readonly ILogger? logger;
		private static RaylibGraphicsEngine instance;

		bool ShouldRun = true;

		public RaylibGraphicsEngine(WindowInitParams initConfig, ILogger? logger = null)
		{
			config = initConfig;
			this.logger = logger;
		}

		public unsafe void Initialize()
		{

			instance = this;
			SetTraceLogCallback(&LogCustom);
			if(config.Borderless)
				SetConfigFlags(ConfigFlags.UndecoratedWindow);

			InitWindow(config.Width, config.Height, config.Title);
			SetTargetFPS(config.TargetFps);

			if (config.Fullscreen)
				ToggleFullscreen();
			if (config.VSync)
				SetConfigFlags(ConfigFlags.VSyncHint);
			
			if(config.MSAA4X)
				SetConfigFlags(ConfigFlags.Msaa4xHint);

			DisableCursor();

			logger?.LogInformation("Graphics Engine Initialized");
			try
			{
				Render();
			}
			catch
			{
				ShouldRun = false;
				CloseWindow();
				logger?.LogError("Graphics Engine stopped running due to an exception.");	
				throw;
			}
		}

		public void Render()
		{
			Camera3D camera = new();
			var control = (CameraController)Engine.Instance.EntityManager.GetEntities().FirstOrDefault(e => e is CameraController);
			camera = control.camera;
			control.OnStart();

			CameraMode cameraMode = CameraMode.FirstPerson;

			//--------------------------------------------------------------------------------------

			bool hasSoundSystem = Engine.Instance.SoundSystem is not null;

			logger?.LogInformation("Starting raylib rendering loop...");
			// Main game loop
			while (ShouldRun)
			{
				control.OnUpdate();	
				if(hasSoundSystem && Engine.Instance.SoundSystem!.IsMusicPlaying())
				{
					UpdateMusicStream((Music) Engine.Instance.SoundSystem.GetMusicPlaying()!);
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

				UpdateCamera(ref camera, cameraMode);
				//----------------------------------------------------------------------------------
				// Draw
				//----------------------------------------------------------------------------------
				BeginDrawing();
				ClearBackground(Color.Black);

				BeginMode3D(control.camera);

				Engine.Instance.EventBus?.NotifyRender();

				
				EndMode3D();

				// Draw info boxes
				DrawRectangle(5, 5, 330, 100, ColorAlpha(Color.SkyBlue, 0.5f));
				DrawRectangleLines(10, 10, 330, 100, Color.Blue);

				DrawText("Camera controls:", 15, 15, 10, Color.Black);
				DrawText("- Move keys: W, A, S, D, Space, Left-Ctrl", 15, 30, 10, Color.Black);
				DrawText("- Look around: arrow keys or mouse", 15, 45, 10, Color.Black);
				DrawText("- Camera mode keys: 1, 2, 3, 4", 15, 60, 10, Color.Black);
				DrawText("- Zoom keys: num-plus, num-minus or mouse scroll", 15, 75, 10, Color.Black);
				DrawText("- Camera projection key: P", 15, 90, 10, Color.Black);

				DrawRectangle(600, 5, 195, 100, Fade(Color.SkyBlue, 0.5f));
				DrawRectangleLines(600, 5, 195, 100, Color.Blue);

				DrawText("Camera status:", 610, 15, 10, Color.Black);
				DrawText($"- Mode: {cameraMode}", 610, 30, 10, Color.Black);
				DrawText($"- Projection: {control.camera.Projection}", 610, 45, 10, Color.Black);
				DrawText($"- Position: {control.camera.Position}", 610, 60, 10, Color.Black);
				DrawText($"- Target: {control.camera.Target}", 610, 75, 10, Color.Black);
				DrawText($"- Up: {control.camera.Up}", 610, 90, 10, Color.Black);

				DrawText($"Physics Elapsed time: {Time.PhysicsDeltaTime}s - Expected: 0.016s", 15, 200, 10, Color.White);
				DrawText($"Update Elapsed time: {Time.DeltaTime}s - Expected: 0.00833s", 15, 220, 10, Color.White);
				DrawText($"Pos: {control.Transform.Position} - {control.camera.Position}", 15, 240, 10, Color.White);
				DrawText($"Rot: {control.Transform.Rotation}", 15, 260, 10, Color.White);

				EndDrawing();
				//----------------------------------------------------------------------------------
			}
			Console.WriteLine($"{ShouldRun}");
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
					instance.logger?.LogError(message);
					break;
				default:
					break;
			}

		}

		public void Shutdown()
		{
			ShouldRun = false;
			logger?.LogWarning("Shutting down graphics engine...");
		}
	}
}
