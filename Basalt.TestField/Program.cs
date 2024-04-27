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
graphicsEngine.PostProcessingShaderCacheKey = "postprocess";
graphicsEngine.LightingShaderCacheKey = "lighting";

RaylibCache.Instance.LoadShader("lighting",
	@"C:\Users\Thiago\source\repos\CSharpTest\bin\Debug\net8.0\resources\shaders\lighting.fs",
	@"C:\Users\Thiago\source\repos\CSharpTest\bin\Debug\net8.0\resources\shaders\lighting.vs");
RaylibCache.Instance.LoadShader("postprocess", "resources/shaders/testshader.fs", string.Empty);

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

var ground = new Entity();
ground.Transform.Position = new Vector3(0, -1, 0);
ground.AddComponent(new ModelRenderer(ground) { ModelCacheKey = "plane", Size = new Vector3(30, 1, 30), ColorTint = Color.Gray });
ground.AddComponent(new BoxCollider(ground) { Size = new Vector3(60, 2, 60) });
ground.AddComponent(new Rigidbody(ground) { IsKinematic = true });

Engine.CreateEntity(ground);

var prop = new Entity();
prop.Transform.Position = new Vector3(3);
prop.AddComponent(new BoxRenderer(prop));
prop.AddComponent(new BoxCollider(prop) { Size = new Vector3(1) });
prop.AddComponent(new Rigidbody(prop) { IsKinematic = false, Mass = 1 });

var propchild = new Entity();
propchild.Transform.Position = prop.Transform.Position + Vector3.UnitY;
propchild.AddComponent(new BoxRenderer(propchild) { Size = new(3)});
propchild.AddComponent(new BoxCollider(propchild) { Size = new Vector3(3) });
prop.AddChildren(propchild);


var player = new CameraController();
player.Id = "entity.player";
Vector3 offset = Vector3.UnitY * -2;
player.Transform.Position = new Vector3(0, 5, 0);
player.AddComponent(new BoxRenderer(player) { Size = new Vector3(1, 1, 1), Color = Color.Red, Offset = offset });
player.AddComponent(new BoxCollider(player) { Size = new Vector3(1, 1, 1), Offset = offset });
player.AddComponent(new Rigidbody(player) { IsKinematic = false, Mass = 25 });
player.AddComponent(new Basalt.TestField.Components.PlayerController(player));
//player.AddComponent(new TrailRenderer(player) { StartRadius = 0.5f, EndRadius = 0.1f, Color = Color.Red, TrailSegmentCount = 25, Offset = offset, TrailRefreshRate = 0.025f });
player.AddComponent(new LightSource(player, "lighting") { Color = Color.Red, Type = LightType.Point });

var json = player.SerializeToJson();
File.WriteAllText("./resources/player.json", json);
var propjson = prop.SerializeToJson();
File.WriteAllText("./resources/prop.json", propjson);


Engine.CreateEntity(player);
Engine.CreateEntity(Entity.DeserializeFromJson(File.ReadAllText("./resources/prop.json")));

RaylibCache.Instance.LoadModel("plane", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources/plane.glb"), "lighting");
RaylibCache.Instance.LoadModel("robot", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources/robot.glb"), "lighting");
Thread engineThread = new Thread(() => engine.Run());
engineThread.Start();
