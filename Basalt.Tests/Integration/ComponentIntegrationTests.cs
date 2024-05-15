using Basalt.Common;
using Basalt.Common.Components;
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

		[SetUp]
		public void Setup()
		{
			var engine = new EngineBuilder()
				.AddComponent<IGraphicsEngine>(() => Mock.Of<IGraphicsEngine>(), true)
				.AddComponent<IEventBus, EventBus>()
				.Build();
		}

		[Test]
		public void ComponentAddedToEngine_ShouldBeSubscribedToEventBus()
		{
			// Arrange
			var entity = new Entity();
			entity.AddComponent(new TestComponent(entity));

			// Act
			Engine.Instance.Initialize();
			Engine.CreateEntity(entity);
			var component = entity.GetComponent<TestComponent>();

			// Assert
			Assert.IsTrue(Engine.Instance.GetEngineComponent<IEventBus>()!.IsSubscribed(component));
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
		public void ComponentCreated_WhenNotAddedToEngine_ShouldNotCallStart()
		{
			// Arrange
			var entity = new Entity();
			entity.AddComponent(new TestComponent(entity));

			// Act
			Engine.Instance.Initialize();

			// Assert
			Assert.IsFalse(entity.GetComponent<TestComponent>().HasStarted);
		}

		[Test]
		public void ComponentDestroy_ShouldUnsubscribe()
		{
			// Arrange
			var entity = new Entity();
			entity.AddComponent(new TestComponent(entity));

			// Act
			Engine.Instance.Initialize();
			Engine.CreateEntity(entity);
			var component = entity.GetComponent<TestComponent>();
			entity.Destroy();

			// Assert
			Assert.IsFalse(Engine.Instance.GetEngineComponent<IEventBus>()!.IsSubscribed(component));
		}

		[Test]
		public void AddComponent_WhenSingleton_ShouldNotCreateAnotherInstance()
		{
			// Arrange
			var entity = new Entity();
			entity.AddComponent(new TestComponent(entity));
			entity.AddComponent(new Rigidbody(entity));

			// Act
			entity.AddComponent(new TestComponent(entity));
			entity.AddComponent(new Rigidbody(entity));
			entity.AddComponent(new Transform(entity));


			// Assert
			Assert.That(entity.GetComponents().Where(c => c.GetType() == typeof(TestComponent)).Count(), Is.EqualTo(1), "Singleton attribute not working");
			Assert.That(entity.GetComponents().Where(c => c.GetType() == typeof(Rigidbody)).Count(), Is.EqualTo(1), "Multiple transforms found");
			Assert.That(entity.GetComponents().Where(c => c.GetType() == typeof(Transform)).Count(), Is.EqualTo(1), "Multiple rigidbodies found");
		}
	}
}
