using Basalt;
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
	TargetFps = 120
};

var logger = new ConsoleLogger(LogLevel.Info);

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

var entity = new Entity();
entity.Transform.Position = new Vector3(0, 15, 0);
entity.AddComponent(new SphereRenderer(entity) { Radius = 5, Color = Color.Yellow});

Engine.CreateEntity(entity);

Thread engineThread = new Thread(() => engine.Run());
engineThread.Start();

soundSystem.LoadAudio("testaudio.mp3", AudioType.SoundEffect);
soundSystem.LoadAudio("testsong.mp3", AudioType.Music);
