using Basalt.Common.Events;
using Basalt.Common.Logging;
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

var logger = new ConsoleLogger(LogLevel.Warning);

var graphicsEngine = new RaylibGraphicsEngine(initParams, logger);

builder.WithGraphicsEngine(graphicsEngine);
builder.WithLogger(logger);

EventBus eventBus = new EventBus();

builder.EventBus = eventBus;

var engine = builder.Build();

engine.Run();

while(true)
{
	if(Raylib.IsKeyPressed(KeyboardKey.G))
	{
		engine.Shutdown();
	}
}
