using Basalt.Core.Common;
using Basalt.Graphics;
using Basalt.Types;

var builder = new EngineBuilder();

var initParams = new WindowInitParams
{
	Title = "Basalt Test Field",
	Width = 1920,
	Height = 1080,
	TargetFps = 10,
	Fullscreen = true,
};

var graphicsEngine = new RaylibGraphicsEngine(initParams);

builder.UseGraphicsEngine(graphicsEngine);

var game = builder.Build();
game.Initialize();