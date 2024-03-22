using Basalt;
using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Common.Events;
using Basalt.Common.Logging;
using Basalt.Common.Physics;
using Basalt.Core.Common.Abstractions.Sound;
using Basalt.Core.Common.Types;
using Basalt.Raylib.Components;
using Basalt.Raylib.Graphics;
using Basalt.Raylib.Sound;
using Basalt.Types;
using Raylib_cs;
using System.Numerics;

var builder = new EngineBuilder();

var initParams = new WindowInitParams
{
	Title = "Basalt Test Field",
	Width = 1280,
	Height = 720,
	TargetFps = 120,
	MSAA4X = true
};

var logger = new ConsoleLogger(LogLevel.Warning);

var graphicsEngine = new RaylibGraphicsEngine(initParams, logger);
var physicsEngine = new PhysicsEngine(logger);
var soundSystem = new RaylibSoundSystem(logger);


builder.WithGraphicsEngine(graphicsEngine);
builder.WithPhysicsEngine(physicsEngine);
builder.WithSoundEngine(soundSystem);

builder.WithLogger(logger);

EventBus eventBus = new EventBus();

builder.EventBus = eventBus;

var engine = builder.Build();

var ground = new Entity();
ground.Transform.Position = new Vector3(0, -1, 0);
ground.AddComponent(new BoxRenderer(ground) { Size = new Vector3(30, 2, 30), Color = Color.Green });
ground.AddComponent(new BoxCollider(ground) { Size = new Vector3(30, 2, 30) });
ground.AddComponent(new Rigidbody(ground) { IsKinematic = true });

Engine.CreateEntity(ground);

var entity = new CameraController();
entity.Transform.Position = new Vector3(0, 5, 0);
entity.AddComponent(new BoxRenderer(entity) { Color = Color.Red, Offset = new(0, -2, 0)});
entity.AddComponent(new BoxCollider(entity) { Size = new Vector3(1, 1, 1), Offset = new Vector3(0, -2, 0) });
entity.AddComponent(new Rigidbody(entity) { IsKinematic = false });

var child1 = new Entity();
child1.Transform.Position = new Vector3(0, 5, 0);
child1.AddComponent(new SphereRenderer(child1) { Radius = 0.5f, Color = Color.Blue, Offset = -Vector3.UnitY * 2 + child1.Transform.Right });
entity.AddChildren(child1);

var child2 = new Entity();
child2.Transform.Position = new Vector3(0, 5, 0);
child2.AddComponent(new SphereRenderer(child1) { Radius = 0.5f, Color = Color.Blue, Offset = -Vector3.UnitY * 2 - child1.Transform.Right });

Engine.CreateEntity(entity);
Engine.CreateEntity(child1);
Engine.CreateEntity(child2);

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
	e.AddComponent(new Rigidbody(e) { Mass = height, Drag = 0.1f, IsKinematic = false });

	Engine.CreateEntity(e);
}




Thread engineThread = new Thread(() => engine.Run());
engineThread.Start();

soundSystem.LoadAudio("testaudio.mp3", AudioType.SoundEffect);
soundSystem.LoadAudio("testsong.mp3", AudioType.Music);
