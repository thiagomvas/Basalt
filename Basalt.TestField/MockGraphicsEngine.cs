using Basalt.Core.Common.Abstractions.Engine;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Basalt.TestField
{
	internal class MockGraphicsEngine : IGraphicsEngine
	{
		public void Initialize()
		{

			const int screenWidth = 800;
			const int screenHeight = 450;

			InitWindow(screenWidth, screenHeight, "raylib [core] example - basic window");

			SetTargetFPS(60);

			Render();
		}

		public void Render()
		{

			// Main game loop
			while (!WindowShouldClose())
			{
				// Update
				//----------------------------------------------------------------------------------
				// TODO: Update your variables here
				//----------------------------------------------------------------------------------

				// Draw
				//----------------------------------------------------------------------------------
				BeginDrawing();
				ClearBackground(Color.RayWhite);

				DrawText("Congrats! You created your first window!", 190, 200, 20, Color.Maroon);

				EndDrawing();
				//----------------------------------------------------------------------------------
			}

			// De-Initialization
			//--------------------------------------------------------------------------------------
			CloseWindow();
			//--------------------------------------------------------------------------------------
		}

		public void Shutdown()
		{

		}
	}
}
