using Basalt.Common;
using Basalt.Common.Events;
using Basalt.Common.Physics;
using Basalt.Core.Common.Abstractions.Engine;
using Moq;

namespace Basalt.Tests.Common
{
	public static class ExtensionMethods
	{
		public static EngineBuilder UseMockPreset(this EngineBuilder builder)
		{
			builder.AddComponent<IGraphicsEngine>(() => Mock.Of<IGraphicsEngine>(), true);
			builder.AddComponent<IEventBus, EventBus>();
			builder.AddComponent<IPhysicsEngine, PhysicsEngine>(true);
			return builder;
		}
	}
}
