using Basalt.Common.Logging;
using Basalt.Core.Common;
using Basalt.Core.Common.Types;
using Basalt.Graphics;
using Basalt.Types;

var builder = new EngineBuilder();

var initParams = new WindowInitParams
{
	Title = "Basalt Test Field",
	Width = 1280,
	Height = 720,
	TargetFps = 120,
};

var logger = new ConsoleLogger(LogLevel.Debug);

var graphicsEngine = new RaylibGraphicsEngine(initParams, logger);
builder.UseGraphicsEngine(graphicsEngine);

var engine = builder.Build();
engine.Logger = logger;

engine.Initialize();