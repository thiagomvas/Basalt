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

for(int i = 0; i < 10; i++)
{

	var p = new Entity();
	p.Id = $"prop{i}";
	p.Transform.Position = new Vector3(Random.Shared.Next(0, 25), i * 2, Random.Shared.Next(0, 25));
	p.AddComponent(new BoxRenderer(p)
	{
		Color = new Color((byte)Random.Shared.Next(0, 255), (byte)Random.Shared.Next(0, 255), (byte)Random.Shared.Next(0, 255), (byte) 255)
	});
	p.AddComponent(new BoxCollider(p) { Size = new Vector3(1) });
	p.AddComponent(new Rigidbody(p) { IsKinematic = false, Mass = 1 });
	Engine.CreateEntity(p);
}
var prop = new Entity();
prop.Id = "prop";
prop.Transform.Position = new Vector3(0, 3, 3);
prop.AddComponent(new BoxRenderer(prop));
prop.AddComponent(new BoxCollider(prop) { Size = new Vector3(1) });
prop.AddComponent(new Rigidbody(prop) { IsKinematic = false, Mass = 1 });


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
//player.AddComponent(new TrailRenderer(player) { StartRadius = 0.5f, EndRadius = 0.1f, Color = Color.Red, TrailSegmentCount = 25, Offset = offset, TrailRefreshRate = 0.025f });
player.AddComponent(new LightSource(player, "lighting") { Color = Color.Red, Type = LightType.Point });

var shoulder1 = new Entity();
shoulder1.Transform.Position = player.Transform.Position + Vector3.UnitX * 0.75f;
shoulder1.AddComponent(new SphereRenderer(shoulder1) { Size = new Vector3(0.5f), Color = Color.Blue, Offset = offset });
shoulder1.AddComponent(new Rigidbody(shoulder1) { IsKinematic = true, Mass = 1 });
player.AddChildren(shoulder1);

var shoulder2 = new Entity();
shoulder2.Transform.Position = player.Transform.Position + Vector3.UnitX * -0.75f;
shoulder2.AddComponent(new SphereRenderer(shoulder2) { Size = new Vector3(0.5f), Color = Color.Blue, Offset = offset });
shoulder2.AddComponent(new Rigidbody(shoulder2) { IsKinematic = true, Mass = 1 });
player.AddChildren(shoulder2);

var hand1 = new Entity();
hand1.Transform.Position = shoulder1.Transform.Position + Vector3.UnitX * 0.75f;
hand1.AddComponent(new SphereRenderer(hand1) { Size = new Vector3(0.5f), Color = Color.Blue });
hand1.AddComponent(new FixedLink(hand1) { AnchoredEntity = shoulder1, Distance = 2 });
hand1.AddComponent(new Rigidbody(hand1) { IsKinematic = false, Mass = 1 });
hand1.AddComponent(new BoxCollider(hand1) { Size = new Vector3(0.5f) });
hand1.AddComponent(new EntityLineRenderer(hand1) { Target = shoulder1, Color = Color.Green, StartRadius = 0.1f, EndRadius = 0.1f, EndOffset = offset });
hand1.AddComponent(new HandController(hand1) { Player = player, Shoulder = shoulder1});
playerHolder.AddChildren(hand1);

var hand2 = new Entity();
hand2.Transform.Position = shoulder2.Transform.Position + Vector3.UnitX * -0.75f;
hand2.AddComponent(new SphereRenderer(hand2) { Size = new Vector3(0.5f), Color = Color.Blue });
hand2.AddComponent(new FixedLink(hand2) { AnchoredEntity = shoulder2, Distance = 2 });
hand2.AddComponent(new Rigidbody(hand2) { IsKinematic = false, Mass = 1 });
hand2.AddComponent(new BoxCollider(hand2) { Size = new Vector3(0.5f) });
hand2.AddComponent(new EntityLineRenderer(hand2) { Target = shoulder2, Color = Color.Green, StartRadius = 0.1f, EndRadius = 0.1f, EndOffset = offset });
//hand2.AddComponent(new HandController(hand2));
playerHolder.AddChildren(hand2);


var json = player.SerializeToJson();
File.WriteAllText("./resources/player.json", json);
var propjson = prop.SerializeToJson();
File.WriteAllText("./resources/prop.json", propjson);


Engine.CreateEntity(playerHolder);
Engine.CreateEntity(Entity.DeserializeFromJson(File.ReadAllText("./resources/prop.json")));

RaylibCache.Instance.LoadModel("plane", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources/plane.glb"), "lighting");
RaylibCache.Instance.LoadModel("robot", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources/robot.glb"), "lighting");
Thread engineThread = new Thread(() => engine.Run());
engineThread.Start();
