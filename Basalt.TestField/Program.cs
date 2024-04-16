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
graphicsEngine.PostProcessingShaderCacheKey = "postprocess";
graphicsEngine.LightingShaderCacheKey = "lighting";

RaylibCache.Instance.LoadShader("lighting",
	@"C:\Users\Thiago\source\repos\CSharpTest\bin\Debug\net8.0\resources\shaders\lighting.vs",
	@"C:\Users\Thiago\source\repos\CSharpTest\bin\Debug\net8.0\resources\shaders\lighting.fs");
//RaylibCache.Instance.LoadShader("postprocess", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources/shaders/grayscale.fs"), string.Empty);

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

var prop = new Entity();
prop.Transform.Position = new Vector3(3);
prop.AddComponent(new BoxRenderer(prop) { Size = new Vector3(3), Color = Color.Blue });
prop.AddComponent(new BoxCollider(prop) { Size = new Vector3(3) });
prop.AddComponent(new Rigidbody(prop) { IsKinematic = false, Mass = 1 });


var player = new CameraController();
player.Id = "entity.player";
Vector3 offset = Vector3.UnitY * -2;
player.Transform.Position = new Vector3(0, 5, 0);
player.AddComponent(new BoxRenderer(player) { Size = new Vector3(1, 1, 1), Color = Color.Red, Offset = offset });
player.AddComponent(new BoxCollider(player) { Size = new Vector3(1, 1, 1), Offset = offset });
player.AddComponent(new Rigidbody(player) { IsKinematic = false, Mass = 25 });
player.AddComponent(new Basalt.TestField.Components.PlayerController(player));
//player.AddComponent(new TrailRenderer(player) { StartRadius = 0.5f, EndRadius = 0.1f, Color = Color.Red, TrailSegmentCount = 25, Offset = offset, TrailRefreshRate = 0.025f });
player.AddComponent(new LightSource(player, "lighting") { Color = Color.Red, Type = LightType.Point });



prop.AddComponent(new EntityLineRenderer(prop) { Color = Color.Red, RenderSideCount = 16, StartRadius = 0.1f, EndRadius = 0.1f, EndOffset = offset, Target = player });
var json = player.SerializeToJson();
File.WriteAllText("./resources/player.json", json);
var propjson = prop.SerializeToJson();
File.WriteAllText("./resources/prop.json", propjson);


Engine.CreateEntity(player);
Engine.CreateEntity(Entity.DeserializeFromJson(File.ReadAllText("./resources/prop.json")));

Entity[] boxes = new Entity[25];
for (int i = 0; i < boxes.Length; i++)
{
	boxes[i] = new Entity();
	boxes[i].Transform.Position = new Vector3(-18, 10 + i, -10);
	boxes[i].Transform.Rotation = new Quaternion(0, -0.70710677f, 0, 0.70710677f);
	boxes[i].AddComponent(new SphereRenderer(boxes[i]) { Size = new Vector3(i == 0 ? 1 : 0.5f), Color = Color.Yellow });
	boxes[i].AddComponent(new BoxCollider(boxes[i]) { Size = new Vector3( i == 0 ? 1 : 0.5f) });

	if(i > 0)
	{
		boxes[i].AddComponent(new Rigidbody(boxes[i]) { Mass = 1, Drag = 0.1f, IsKinematic = false });
		boxes[i].AddComponent(new ChainLink(boxes[i]) { AnchoredEntity = boxes[i - 1], MaxDistance = 1f, JointForceMultiplier = 10 });
		boxes[i].AddComponent(new EntityLineRenderer(boxes[i]) { Color = Color.DarkGreen, Target = boxes[i - 1], RenderSideCount = 16, StartRadius = 0.05f, EndRadius = 0.05f });
		if(i == boxes.Length - 1)
		{
			boxes[i].Transform.IsFixedPoint = true;

		}
	}
	else
	{
		boxes[i].AddComponent(new Rigidbody(boxes[i]) { Mass = 1, Drag = 0.1f, IsKinematic = true });
		boxes[i].Transform.IsFixedPoint = true;
	}

	Engine.CreateEntity(boxes[i]);
}

RaylibCache.Instance.LoadModel("test", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources/dodecahedron.obj"), "lighting");

Thread engineThread = new Thread(() => engine.Run());
engineThread.Start();
