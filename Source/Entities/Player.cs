using Raylib_cs;
using System.Numerics;
using Basalt.Source.Components;
using Basalt.Source.Core.Utils;
using Basalt.Source.Core;
using Basalt.Source.Core.Types;

namespace Basalt.Source.Entities
{
    /// <summary>
    /// Represents a player in the game.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Gets or sets the GameObject associated with the player.
        /// </summary>
        public GameObject gameObject;

        public Vector3 Movement;

        /// <summary>
        /// Gets the unique identifier for the player.
        /// </summary>
        public int id;

        /// <summary>
        /// Gets or sets the movement speed of the player.
        /// </summary>
        public int MovementSpeed = 25;

        /// <summary>
        /// Gets the Rigidbody component associated with the player's GameObject.
        /// </summary>
        Rigidbody rb;

        ObjectPooling pool = new();

        /// <summary>
        /// Initializes a new instance of the Player class with the provided GameObject.
        /// </summary>
        /// <param name="gameObject">The GameObject associated with the player.</param>
        public Player(GameObject gameObject)
        {
            this.gameObject = gameObject;
            id = 0;
            Engine.OnUpdate += OnMovePlayer;
            rb = gameObject.GetComponent<Rigidbody>();


        }

        /// <summary>
        /// Callback method for handling player movement.
        /// </summary>
        public void OnMovePlayer() => MovePlayer();

        /// <summary>
        /// Moves the player based on input controls and mouse position.
        /// </summary>
        public void MovePlayer()
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE)) gameObject.IsActive = !gameObject.IsActive;

            if (!gameObject.IsActive) return;
            Movement = new Vector3((Raylib.IsKeyDown(KeyboardKey.KEY_W) || Raylib.IsKeyDown(KeyboardKey.KEY_UP) ? 1 : 0) * MovementSpeed * Time.DeltaTime -      // Move forward-backward
                                (Raylib.IsKeyDown(KeyboardKey.KEY_S) || Raylib.IsKeyDown(KeyboardKey.KEY_DOWN) ? 1 : 0) * MovementSpeed * Time.DeltaTime,
                                (Raylib.IsKeyDown(KeyboardKey.KEY_D) || Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT) ? 1 : 0) * MovementSpeed * Time.DeltaTime -   // Move right-left
                                (Raylib.IsKeyDown(KeyboardKey.KEY_A) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT) ? 1 : 0) * MovementSpeed * Time.DeltaTime,
                                0.0f);

            //gameObject.Transform.MoveTo(Engine.Camera.Position); // See Issue #19
            if (Raylib.IsKeyDown(KeyboardKey.KEY_R)) gameObject.Transform.MoveTo(Vector3.Zero);

            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_RIGHT))
            {
                var obj = pool.Get();
                obj.Transform.Position = gameObject.Transform.Position + gameObject.Transform.Forward * 25;
                obj.GetComponent<Projectile>().Velocity = gameObject.Transform.Forward * 10;
                obj.Transform.Rotation = gameObject.Transform.Rotation;
            }



            Vector3 rotation = new(Raylib.GetMouseDelta().X * 0.05f,                            // Rotation: yaw
                                   Raylib.GetMouseDelta().Y * 0.05f,                            // Rotation: pitch
                                   0.0f);                                             // Rotation: roll

            Raylib.UpdateCameraPro(ref Engine.CurrentScene.Cameras[0].Camera3D,
                            Movement,
                            rotation,
                            0);

            //Vector2 mouseCoordsOnWorld = ScreenToWorldPosition(Raylib.GetMousePosition(), Engine.Camera.Camera2D);

            //gameObject.Transform.Rotation = LookAtRotation(gameObject.Transform.Position, Conversions.XYToVector3(mouseCoordsOnWorld), Vector3.UnitY);
        }
    }

}
