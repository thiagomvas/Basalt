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
	internal class EntityIntegrationTests
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
		public void EntityAddComponent_WhenAddingRigidbody_ShouldUpdateField()
		{
			// Arrange
			var entity = new Entity();
			entity.AddComponent(new Rigidbody(entity));

			// Act
			Engine.Instance.Initialize();
			Engine.CreateEntity(entity);

			// Assert
			Assert.IsNotNull(entity.Rigidbody);
			Assert.That(entity.Rigidbody, Is.EqualTo(entity.GetComponent<Rigidbody>()));
		}

		[Test]
		public void EntityRemoveComponent_ShouldRemoveFromEventBus()
		{
			// Arrange
			var entity = new Entity();
			entity.AddComponent(new TestComponent(entity));

			// Act
			Engine.Instance.Initialize();
			Engine.CreateEntity(entity);
			var component = entity.GetComponent<TestComponent>();
			entity.RemoveComponent(component);

			// Assert
			Assert.IsFalse(Engine.Instance.GetEngineComponent<IEventBus>()!.IsSubscribed(component));
		}

		[Test]
		public void EntityRemoveComponent_WhenRemovingRigidbody_ShouldUpdateField()
		{
			// Arrange
			var entity = new Entity();
			entity.AddComponent(new Rigidbody(entity));
			entity.AddComponent(new TestComponent(entity));

			// Act
			Engine.Instance.Initialize();
			Engine.CreateEntity(entity);
			entity.RemoveComponent(entity.Rigidbody);

			// Assert
			Assert.IsNull(entity.Rigidbody);
		}

		[Test]
		public void EntityDestroy_ShouldUnsubscribeAllComponents()
		{
			// Arrange
			var entity = new Entity();
			entity.AddComponent(new TestComponent(entity));
			entity.AddComponent(new Rigidbody(entity));

			// Act
			Engine.Instance.Initialize();
			Engine.CreateEntity(entity);
			entity.Destroy();

			// Assert
			Assert.IsFalse(Engine.Instance.GetEngineComponent<IEventBus>()!.IsSubscribed(entity.GetComponent<TestComponent>()));
			Assert.IsFalse(Engine.Instance.GetEngineComponent<IEventBus>()!.IsSubscribed(entity.GetComponent<Rigidbody>()));
		}

		[Test]
		public void EntitySerializeToJson_ShouldSerializeAllComponents()
		{
			// Arrange
			var entity = new Entity();
			entity.AddComponent(new TestComponent(entity));
			entity.AddComponent(new Rigidbody(entity));

			// Act
			Engine.Instance.Initialize();
			Engine.CreateEntity(entity);
			var json = entity.SerializeToJson();

			// Assert
			Assert.IsNotNull(json);
			Assert.That(json, Does.Contain("TestComponent"));
			Assert.That(json, Does.Contain("Rigidbody"));
		}

		[Test]
		public void EntityDeserializeFromJson_ShouldDeserializeAllComponents()
		{
			// Arrange
			var entity = new Entity();
			entity.AddComponent(new TestComponent(entity));
			entity.AddComponent(new Rigidbody(entity));

			// Act
			Engine.Instance.Initialize();
			Engine.CreateEntity(entity);
			var json = entity.SerializeToJson();
			var newEntity = Entity.DeserializeFromJson(json);

			// Assert
			Assert.IsNotNull(newEntity);
			Assert.IsNotNull(newEntity.GetComponent<TestComponent>());
			Assert.IsNotNull(newEntity.GetComponent<Rigidbody>());
		}

		[Test]
		public void EntityGetComponent_WhenComponentExists_ShouldReturnComponent()
		{
			// Arrange
			var entity = new Entity();
			entity.AddComponent(new TestComponent(entity));

			// Act
			Engine.Instance.Initialize();
			Engine.CreateEntity(entity);
			var component = entity.GetComponent<TestComponent>();

			// Assert
			Assert.IsNotNull(component);
		}

		[Test]
		public void EntityGetComponent_WhenComponentDoesNotExist_ShouldReturnNull()
		{
			// Arrange
			var entity = new Entity();

			// Act
			Engine.Instance.Initialize();
			Engine.CreateEntity(entity);
			var component = entity.GetComponent<TestComponent>();

			// Assert
			Assert.IsNull(component);
		}

		[Test]
		public void EntitySerializeToJson_WhenComponentHasEntityReference_ShouldSerializeReference()
		{
			// Arrange
			var target = new Entity();
			target.Id = "entity.target";
			var entity = new Entity();
			entity.Id = "entity.testing";
			entity.AddComponent(new TestComponent(entity) { Target = target });

			// Act
			Engine.Instance.Initialize();
			Engine.CreateEntity(entity);
			var json = entity.SerializeToJson();

			// Assert
			Assert.IsNotNull(json);
			Assert.That(json, Does.Contain("entity.target"));
		}

		[Test]
		public void EntityDeserializeFromJson_WhenComponentHasEntityReference_ShouldDeserializeReference()
		{
			// Arrange
			var target = new Entity();
			target.Id = "entity.target";
			var entity = new Entity();
			entity.Id = "entity.testing";
			entity.AddComponent(new TestComponent(entity) { Target = target });

			// Act
			Engine.Instance.Initialize();
			Engine.CreateEntity(target);
			var json = entity.SerializeToJson();
			var newEntity = Entity.DeserializeFromJson(json);
			Engine.CreateEntity(newEntity);

			// Assert
			Assert.IsNotNull(newEntity, "Deserialization failed, result entity was null");
			Assert.IsNotNull(newEntity.GetComponent<TestComponent>(), "Test component was null");
			Assert.IsTrue(newEntity.GetComponent<TestComponent>().HasStarted, "Component did not initialize");
			Assert.IsNotNull(newEntity.GetComponent<TestComponent>().Target, "Target was null");
			Assert.That(newEntity.GetComponent<TestComponent>().Target.Id, Is.EqualTo(target.Id), "Ids are different");
		}

	}
}
