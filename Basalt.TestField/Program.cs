using Basalt.Common.Events;
using Basalt.Common.Logging;
using Basalt.Common.Physics;
using Basalt.Core.Common;
using Basalt.Core.Common.Types;
using Basalt.Graphics;
using Basalt.Types;
using Raylib_cs;

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

builder.WithGraphicsEngine(graphicsEngine);
builder.WithPhysicsEngine(physicsEngine);
builder.WithLogger(logger);

EventBus eventBus = new EventBus();

builder.EventBus = eventBus;

var engine = builder.Build();

engine.Run();

