using Basalt;
using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Common.Events;
using Basalt.Common.Logging;
using Basalt.Common.Physics;
using Basalt.Core.Common.Abstractions.Input;
using Basalt.Core.Common.Abstractions.Sound;
using Basalt.Core.Common.Types;
using Basalt.Raylib.Components;
using Basalt.Raylib.Graphics;
using Basalt.Raylib.Input;
using Basalt.Raylib.Sound;
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
	Fullscreen = true,
	MSAA4X = true,
	PostProcessing = false,
};

var logger = new ConsoleLogger(LogLevel.Info);

var graphicsEngine = new RaylibGraphicsEngine(initParams, logger);
graphicsEngine.PostProcessingFragmentShaderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources/shaders/grayscale.fs");

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
ground.AddComponent(new BoxRenderer(ground) { Size = new Vector3(30, 2, 30), Color = Color.Gray });
ground.AddComponent(new BoxCollider(ground) { Size = new Vector3(30, 2, 30) });
ground.AddComponent(new Rigidbody(ground) { IsKinematic = true });

Engine.CreateEntity(ground);

var box = new Entity();
box.Transform.Position = new Vector3(0, 10, 0);
box.Transform.Rotation = new Quaternion(0, -0.70710677f, 0, 0.70710677f);
box.AddComponent(new Rigidbody(box) { Mass = 1, Drag = 0.1f, IsKinematic = false });
box.AddComponent(new BoxRenderer(box) { Size = new Vector3(1, 1, 1), Color = Color.Yellow });
box.AddComponent(new BoxCollider(box) { Size = new Vector3(1, 1, 1) });

var player = new CameraController();
Vector3 offset = Vector3.UnitY * -2;
player.Transform.Position = new Vector3(0, 5, 0);
player.AddComponent(new BoxCollider(player) { Size = new Vector3(1, 1, 1), Offset = offset });
player.AddComponent(new BoxRenderer(player) { Size = new Vector3(1, 1, 1), Color = Color.Red, Offset = offset });
player.AddComponent(new Rigidbody(player) { IsKinematic = false, Mass = 5 });
player.AddComponent(new Basalt.TestField.Components.PlayerController(player));
player.AddComponent(new FixedLink(player) { AnchoredEntity = box, Distance = 10 });
player.AddComponent(new TrailRenderer(player) { StartRadius = 0.5f, EndRadius = 0.1f, Color = Color.Red, TrailSegmentCount = 25, Offset = offset, TrailRefreshRate = 0.025f });
player.AddComponent(new EntityLineRenderer(player) { Color = Color.DarkGreen, StartOffset = offset, Target = box, RenderSideCount = 16, StartRadius = 0.1f, EndRadius = 0.1f });

var json = player.SerializeToJson();
File.WriteAllText("player.json", json);

Engine.CreateEntity(player);
Engine.CreateEntity(box);



int MaxColumns = 12;

for (int i = 0; i < MaxColumns; i++)
{
	var height = Random.Shared.NextSingle() * 12;
	var position = new Vector3(Random.Shared.NextSingle() * 30 - 15, height / 2, Random.Shared.NextSingle() * 30 - 15);
	var color = new Color(Random.Shared.Next(20, 255), Random.Shared.Next(10, 55), 30, 255);

	Entity e = new();
	e.Transform.Position = position;
	e.AddComponent(new BoxRenderer(e) { Size = new Vector3(2, height, 2), Color = color });
	e.AddComponent(new BoxCollider(e) { Size = new Vector3(2, height, 2) });
	e.AddComponent(new Rigidbody(e) { Mass = height, Drag = 0.1f, IsKinematic = true });

	Engine.CreateEntity(e);
}

Thread engineThread = new Thread(() => engine.Run());
engineThread.Start();

soundSystem.LoadAudio("testaudio.mp3", AudioType.SoundEffect);
soundSystem.LoadAudio("testsong.mp3", AudioType.Music);

