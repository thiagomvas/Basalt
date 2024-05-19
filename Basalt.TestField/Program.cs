using Basalt;
using Basalt.Common;
using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Common.Events;
using Basalt.Common.Logging;
using Basalt.Common.Physics;
using Basalt.Core.Common.Abstractions.Engine;
using Basalt.Core.Common.Abstractions.Input;
using Basalt.Math;
using Basalt.Raylib.Components;
using Basalt.Raylib.Graphics;
using Basalt.Raylib.Input;
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

var builder = new EngineBuilder();

builder.AddComponent<IGraphicsEngine, RaylibGraphicsEngine>(() => new RaylibGraphicsEngine(initParams), true);
builder.AddComponent<IPhysicsEngine, PhysicsEngine>(true);
builder.AddComponent<IEventBus, EventBus>();
builder.AddComponent<IInputSystem, RaylibInputSystem>();

builder.AddLogger(new ConsoleLogger());

var engine = builder.Build();

engine.Initialize();

var ground = new Entity();
ground.Transform.Position = new Vector3(0, -1, 0);
ground.AddComponent(new BoxRenderer(ground) { Size = new Vector3(60, 1, 60), Color = Color.Gray });
ground.AddComponent(new BoxCollider(ground) { Size = new Vector3(60, 1, 60) });
ground.AddComponent(new Rigidbody(ground) { IsKinematic = true });
ground.Id = "ground";

Engine.CreateEntity(ground);


// Instantiate boxes randomly on top of grund
for (int i = 0; i < 50; i++)
{
	var box = new Entity();
	box.Transform.Position = new Vector3(Random.Shared.Next(-15, 15), 25 + i, Random.Shared.Next(-15, 15));
	box.AddComponent(new BoxRenderer(box) { Size = new Vector3(1, 1, 1), Color = Color.Blue });
	box.AddComponent(new BoxCollider(box) { Size = new Vector3(1, 1, 1) });
	box.AddComponent(new Rigidbody(box) { IsKinematic = false, Mass = 1 });
	box.Id = $"box{i}";

	Engine.CreateEntity(box);
}



var ground2 = new Entity();
ground2.Transform.Position = new Vector3(150, -1, 0);
ground2.AddComponent(new BoxRenderer(ground2) { Size = new Vector3(60, 1, 60), Color = Color.Gray });
ground2.AddComponent(new BoxCollider(ground2) { Size = new Vector3(60, 1, 60) });
ground2.AddComponent(new Rigidbody(ground2) { IsKinematic = true });
ground2.Id = "ground2";

Engine.CreateEntity(ground2);

// Instantiate boxes randomly on top of ground2

for (int i = 0; i < 20; i++)
{
	var box = new Entity();
	box.Transform.Position = new Vector3(150 + Random.Shared.Next(-5, 5), 25 + i, Random.Shared.Next(-5, 5));
	box.AddComponent(new BoxRenderer(box) { Size = new Vector3(1, 1, 1), Color = Color.Blue });
	box.AddComponent(new BoxCollider(box) { Size = new Vector3(1, 1, 1) });
	box.AddComponent(new Rigidbody(box) { IsKinematic = false, Mass = 1 });
	box.Id = $"box2{i}";

	Engine.CreateEntity(box);
}


var player = new Entity();
player.AddComponent(new CameraController(player));
player.Id = "entity.player";
Vector3 offset = Vector3.UnitY * -1;
player.Transform.Position = new Vector3(0, 5, 0);
player.AddComponent(new SphereRenderer(player) { Size = new Vector3(1f), Color = Color.Red, Offset = offset });
player.AddComponent(new BoxCollider(player) { Size = new Vector3(1, 2, 1), Offset = offset });
player.AddComponent(new Rigidbody(player) { IsKinematic = false, Mass = 25 });
player.AddComponent(new Basalt.TestField.Components.PlayerController(player));
//player.AddComponent(new TrailRenderer(player) { StartRadius = 0.5f, EndRadius = 0.1f, Color = Color.Red, TrailSegmentCount = 25, Offset = offset, TrailRefreshRate = 0.025f });
player.AddComponent(new LightSource(player, "lighting") { Color = Color.Red, Type = LightType.Point });

Engine.CreateEntity(player);
