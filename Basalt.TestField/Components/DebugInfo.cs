using Basalt.Common;
using Basalt.Common.Components;
using Basalt.Common.Entities;
using static Raylib_cs.Raylib;

namespace Basalt.TestField.Components
{
	internal class DebugInfo : UIComponent
	{
		private Queue<float> pastPhysicsDeltaTime = new Queue<float>(60);
		private Queue<float> pastFps = new Queue<float>(300);
		public DebugInfo(Entity entity) : base(entity)
		{
		}

		public override void OnStart()
		{

		}

		public override void OnUpdate()
		{
			pastFps.Enqueue(GetFPS());
			if (pastFps.Count > 300)
				pastFps.Dequeue();

		}

		public override void OnPhysicsUpdate()
		{
			pastPhysicsDeltaTime.Enqueue(Time.PhysicsDeltaTime * 1000);
			if (pastPhysicsDeltaTime.Count > 60)
				pastPhysicsDeltaTime.Dequeue();
		}

		public override void OnUIRender()
		{
			DrawFPS(10, 10);
			//DrawText($"Entities: {Engine.Instance.EntityManager.EntityCount}", 10, 30, 18, Color.DarkGreen);
			//DrawText($"Threads Used: {Process.GetCurrentProcess().Threads.Count}", 10, 50, 18, Color.DarkGreen);
			//DrawText($"RAM Used: {Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024} MB", 10, 70,18, Color.DarkGreen);
			//DrawText($"Average Physics DeltaTime: {pastPhysicsDeltaTime.Average()} ms", 10, 90, 18, Color.DarkGreen);
			//DrawText($"Average FPS: {pastFps.Average()}", 10, 110, 18, Color.DarkGreen);
		}
	}
}
