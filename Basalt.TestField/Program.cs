using Basalt;
using Basalt.Common;
using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Common.Events;
using Basalt.Common.Logging;
using Basalt.Common.Physics;
using Basalt.Core.Common.Abstractions.Engine;
using Basalt.Core.Common.Abstractions.Input;
using Basalt.Core.Common.Abstractions.Sound;
using Basalt.Core.Common.Types;
using Basalt.Raylib.Components;
using Basalt.Raylib.Graphics;
using Basalt.Raylib.Input;
using Basalt.Raylib.Sound;
using Basalt.Raylib.Utils;
using Basalt.TestField;
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

var builder = new EngineBuilder();

builder.AddComponent<IGraphicsEngine, MockGraphicsEngine> (() => new MockGraphicsEngine(), true);
builder.AddComponent<IEventBus, EventBus>(() => new EventBus());

builder.AddLogger(new ConsoleLogger());

var engine = builder.Build();
engine.Initialize();