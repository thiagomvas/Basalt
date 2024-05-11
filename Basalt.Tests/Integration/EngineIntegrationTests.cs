using Basalt.Common;
using Basalt.Common.Entities;
using Basalt.Common.Events;
using Basalt.Core.Common.Abstractions.Engine;
using Basalt.Tests.Common;
using Moq;
using System.Security.Cryptography.X509Certificates;

namespace Basalt.Tests.Integration
{
	[TestFixture]
	public class EngineIntegrationTests
	{
		[Test]
		public void TestEngineInitialization()
		{
			// Arrange
			var logger = new Mock<ILogger>();

			var engine = new EngineBuilder()
				.AddComponent<IGraphicsEngine>(() => Mock.Of<IGraphicsEngine>(), true)
				.AddComponent<IEventBus, EventBus>()
				.AddComponent<TestEngineComponent>(() => new() { Value = 10})
				.AddLogger(logger.Object)
				.Build();

			// Act
			engine.Initialize();

			// Assert
			Assert.IsTrue(engine.Running, "Engine did not change running bool");
			Assert.IsNotNull(engine.EntityManager, "Entity manager was null");
			Assert.IsNotNull(engine.GetEngineComponent<IGraphicsEngine>());
			Assert.IsNotNull(engine.GetEngineComponent<IEventBus>());
			Assert.IsNotNull(engine.GetEngineComponent<TestEngineComponent>());
			Assert.That(engine.GetEngineComponent<TestEngineComponent>()!.Value, Is.EqualTo(10)); 
			Assert.IsTrue(engine.GetEngineComponent<TestEngineComponent>()!.Initialized);
		}

		[Test]
		public void TestEngineFailsToStartWithoutGraphicsEngine()
		{
			// Arrange
			var loggerMock = new Mock<ILogger>();

			var engineBuilder = new EngineBuilder()
									.AddComponent<IEventBus>(() => Mock.Of<IEventBus>())
									.AddLogger(loggerMock.Object);
			var engine = engineBuilder.Build();

			// Act
			engine.Initialize();

			// Assert
			Assert.IsFalse(engine.Running, "Engine should not be running");
		}

		[Test]
		public void TestEngineFailsToStartWithoutEventBus()
		{
			// Arrange
			var loggerMock = new Mock<ILogger>();

			var engineBuilder = new EngineBuilder()
									.AddComponent<IGraphicsEngine>(() => Mock.Of<IGraphicsEngine>())
									.AddLogger(loggerMock.Object);
			var engine = engineBuilder.Build();

			// Act
			engine.Initialize();

			// Assert
			Assert.IsFalse(engine.Running, "Engine should not be running");
		}

		[Test]
		public void TestEngineShutdown()
		{
			// Arrange
			var engine = new EngineBuilder()
				.AddComponent<IGraphicsEngine>(() => Mock.Of<IGraphicsEngine>(), true)
				.AddComponent<IEventBus, EventBus>()
				.AddComponent<TestEngineComponent>(() => new() { Value = 10})
				.Build();

			engine.Initialize();

			// Act
			engine.Shutdown();

			// Assert
			Assert.IsFalse(engine.Running, "Engine did not change running bool");
			Assert.IsFalse(engine.GetEngineComponent<TestEngineComponent>()!.Initialized);
		}

		[Test]
		public void TestEngineCreateEntity()
		{
			// Arrange
			var entityPostInit = new Entity();
			var entityPreInit = new Entity();

			entityPostInit.Id = "entity.post";
			entityPreInit.Id = "entity.pre";

			var engine = new EngineBuilder()
				.AddComponent<IGraphicsEngine>(() => Mock.Of<IGraphicsEngine>(), true)
				.AddComponent<IEventBus, EventBus>()
				.Build();

			Engine.CreateEntity(entityPreInit);

			engine.Initialize();


			// Act
			Engine.CreateEntity(entityPostInit);

			// Assert
			Assert.IsNotNull(entityPostInit, "Entity was null");
			Assert.That(engine.EntityManager.GetEntities().Count, Is.EqualTo(2));
			Assert.IsNotNull(engine.EntityManager.GetEntity("entity.post"));
			Assert.IsNotNull(engine.EntityManager.GetEntity("entity.pre"));
		}

		[Test]
		public void TestEventBusNotifications()
		{
			// Arrange
			var entity = new Entity();
			entity.AddComponent(new TestComponent(entity));

			var engine = new EngineBuilder()
				.AddComponent<IGraphicsEngine>(() => Mock.Of<IGraphicsEngine>(), true)
				.AddComponent<IEventBus, EventBus>()
				.Build();

			engine.Initialize();

			Engine.CreateEntity(entity);

			int physicsCalls = 10;
			int updareCalls = 12;
			int renderCalls = 12;

			var eventBus = engine.GetEngineComponent<IEventBus>();

			// Act
			eventBus?.NotifyStart();
			for (int i = 0; i < physicsCalls; i++)
			{
				eventBus?.NotifyPhysicsUpdate();
			}

			for (int i = 0; i < updareCalls; i++)
			{
				eventBus?.NotifyUpdate();
			}

			for (int i = 0; i < renderCalls; i++)
			{
				eventBus?.NotifyRender();
			}

			// Assert
			Assert.That(entity.GetComponent<TestComponent>()!.OnStartCount, Is.EqualTo(1));
			Assert.That(entity.GetComponent<TestComponent>()!.OnPhysicsUpdateCount, Is.EqualTo(physicsCalls));
			Assert.That(entity.GetComponent<TestComponent>()!.OnUpdateCount, Is.EqualTo(updareCalls));
			Assert.That(entity.GetComponent<TestComponent>()!.OnRenderCount, Is.EqualTo(renderCalls));
		}
	}
}
