using Basalt.Common;
using Basalt.Common.Entities;
using Basalt.Common.Events;
using Basalt.Core.Common.Abstractions.Engine;
using Basalt.Tests.Common;
using Moq;

namespace Basalt.Tests.Integration
{
	[TestFixture]
	internal class ComponentIntegrationTests
	{
		private IEventBus? eventBus;

		[OneTimeSetUp]
		public void Setup()
		{



			var engine = new EngineBuilder()
				.AddComponent<IGraphicsEngine>(() => Mock.Of<IGraphicsEngine>(), true)
				.AddComponent<IEventBus, EventBus>()
				.Build();

			eventBus = engine.GetEngineComponent<IEventBus>();
			
		}

		[Test]
		public void ComponentCreated_WhenEngineRunning_ShouldCallStart()
		{
			// Arrange
			var entity = new Entity();
			entity.AddComponent(new TestComponent(entity));

			// Act
			Engine.Instance.Initialize();
			Engine.CreateEntity(entity);

			// Assert
			Assert.IsTrue(entity.GetComponent<TestComponent>().HasStarted);
		}

		[Test]
		public void ComponentCreated_WhenEngineNotRunning_ShouldCallStartOnInit()
		{
			// Arrange
			var entity = new Entity();
			entity.AddComponent(new TestComponent(entity));
			Engine.CreateEntity(entity);

			// Act
			bool beforeInit = entity.GetComponent<TestComponent>().HasStarted;
			Engine.Instance.Initialize();
			bool afterInit = entity.GetComponent<TestComponent>().HasStarted;

			// Assert
			Assert.IsFalse(beforeInit);
			Assert.IsTrue(afterInit);
		}
	}
}
