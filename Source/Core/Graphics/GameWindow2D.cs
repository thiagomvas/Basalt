﻿using Basalt.Source.Components;
using Basalt.Source.Core.Types;
using Basalt.Source.Core.Utils;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Basalt.Source.Core.Graphics
{
    public class GameWindow2D : GameWindow
    {
        public override void Init(int Width, int Height, Camera cameraObject)
        {
            Debug.Setup();
            SetConfigFlags(ConfigFlags.FLAG_WINDOW_RESIZABLE);
            SetConfigFlags(ConfigFlags.FLAG_WINDOW_MAXIMIZED);

            InitWindow(Width, Height, "New Game");

            // 2D Camera used on the game
            Camera2D defaultCamera = new Camera2D();
            defaultCamera.Rotation = 0.0f;
            defaultCamera.Zoom = 1.0f;

            foreach (var obj in Globals.GameObjectsOnScene)
            {
                if (obj.TryGetComponent(out Renderer rend)) RenderWorldSpace += rend.OnRender;
            }




            while (!WindowShouldClose())
            {

                if (IsKeyPressed(KeyboardKey.KEY_F1)) Debug.ToggleDebug(); // Temporary
                if (Debug.IsDebugEnabled && IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
                    Debug.SelectedNearestGameObject(GetScreenToWorld2D(GetMousePosition(), cameraObject.Camera2D));


                BeginDrawing();
                ClearBackground(BackgroundColor);

                BeginMode2D(cameraObject.Camera2D); // Setting the camera view | Anything drawn inside Mode2D will be affected by the camera's POV

                CallRenderWorldSpace();
                EndMode2D();

                Debug.DrawDebugUI();

                EndDrawing();

            }

        }


        /// <summary>
        /// Draws all the UI;
        /// </summary>
        private void DrawUI()
        {
            int i = 0;
            //foreach (var pair in Globals.AllComponentsOnScene)
            //{
            //    DrawText($"{pair.Key} : {pair.Value.Count}", 12, 30 + 15 * i, 20, FontColor);
            //    i++;
            //}

            foreach (var obj in Globals.GameObjectsOnScene)
            {
                DrawText($"Position of object #{i} {obj.Transform.Position}", 400, 30 + 15 * i, 20, FontColor);
                i++;
            }

            foreach (var element in Globals.UIElementsOnScene)
                element.Render();


        }

    }
}
