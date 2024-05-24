using Basalt.Common;
using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Common.Events;
using Basalt.Common.Utils;
using Basalt.Core.Common.Abstractions.Engine;
using Basalt.Tests.Common;
using Moq;
using System.Numerics;

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
		public void ComponentAddedToEntity_ShouldBeSubscribedToEventBus()
		{
			// Arrange
			var entity = new Entity();
			entity.AddComponent(new TestComponent(entity));

			// Act
			Engine.Instance.Initialize();
			Engine.CreateEntity(entity);
			var component = entity.GetComponent<TestComponent>();
			Engine.Instance.GetEngineComponent<IEventBus>()!.TriggerEvent(BasaltConstants.StartEventKey);


			// Assert
			Assert.IsTrue(component.HasStarted);
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
			entity.AddComponent(new Rigidbody(entity));
			IEqualityComparer<Vector3> comparer = new Vector3EqualityComparer();

			// Act
			Engine.Instance.Initialize();
			Engine.CreateEntity(entity);
			var component = entity.GetComponent<TestComponent>();
			entity.Destroy();
			Engine.Instance.GetEngineComponent<IEventBus>()!.TriggerEvent(BasaltConstants.PhysicsUpdateEventKey);

			// Assert
			Assert.That(entity.Transform.Position, Is.EqualTo(Vector3.Zero).Using(comparer));
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
