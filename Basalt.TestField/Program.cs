using Basalt;
using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Common.Events;
using Basalt.Common.Logging;
using Basalt.Common.Physics;
using Basalt.Core.Common.Types;
using Basalt.Raylib.Components;
using Basalt.Raylib.Graphics;
using Basalt.Raylib.Input;
using Basalt.Raylib.Sound;
using Basalt.Raylib.Utils;
using Basalt.TestField;
using Basalt.TestField.Components;
using Basalt.Types;
using Raylib_cs;
using System.Numerics;


var initParams = new WindowInitParams
{
	Title = "Basalt Test Field",
	Width = 1920,
	Height = 1080,
	TargetFps = 120,
	Fullscreen = false,
	Borderless = true,
	MSAA4X = true,
	PostProcessing = false,
};

var logger = new ConsoleLogger(LogLevel.Info);

var graphicsEngine = new RaylibGraphicsEngine(initParams, logger);
graphicsEngine.LightingShaderCacheKey = "lighting";

RaylibCache.Instance.LoadShader("lighting",
	@"C:\Users\Thiago\source\repos\Basalt\Basalt.TestField\bin\Release\net8.0\resources\shaders\lighting.fs",
	@"C:\Users\Thiago\source\repos\Basalt\Basalt.TestField\bin\Release\net8.0\resources\shaders\lighting.vs");

var physicsEngine = new PhysicsEngine(logger);
var soundSystem = new RaylibSoundSystem(logger);


EventBus eventBus = new EventBus();

RaylibInputSystem inputSystem = new();


Engine engine = new()
{
	GraphicsEngine = graphicsEngine,
	PhysicsEngine = physicsEngine,
	SoundSystem = soundSystem,
	Logger = logger,
	EventBus = eventBus,
	InputSystem = inputSystem
};


var playerHolder = new Entity();

var player = new CameraController();
playerHolder.AddChildren(player);
player.Id = "entity.player";
Vector3 offset = Vector3.UnitY * -1;
player.Transform.Position = new Vector3(0, 5, 0);
player.AddComponent(new SphereRenderer(player) { Size = new Vector3(1f), Color = Color.Red, Offset = offset });
player.AddComponent(new BoxCollider(player) { Size = new Vector3(1, 2, 1), Offset = offset });
player.AddComponent(new Rigidbody(player) { IsKinematic = false, Mass = 25 });
player.AddComponent(new Basalt.TestField.Components.PlayerController(player));


var sun = new Entity();
sun.Transform.Position = new Vector3(250, 50, 0);
sun.AddComponent(new LightSource(sun, "lighting") { Color = new Color((byte) 255, (byte) 238, (byte) 80, (byte) 255) });
sun.AddComponent(new SphereRenderer(sun) { Size = new Vector3(10f), Color = Color.Yellow });
Engine.CreateEntity(sun);


Engine.CreateEntity(playerHolder);

RaylibCache.Instance.LoadModel("plane", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources/plane.glb"), "lighting");
RaylibCache.Instance.LoadModel("robot", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources/robot.glb"), "lighting");


Thread engineThread = new Thread(() => engine.Run());
engineThread.Start();
