using Basalt.Common;
using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Common.Events;
using Basalt.Core.Common.Abstractions.Engine;
using Basalt.Tests.Common;
using Moq;
using System.Numerics;
using System.Text.RegularExpressions;

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
		public void EntityRemoveComponent_WhenRemovingRigidbody_ShouldUpdateField()
		{
			// Arrange
			var entity = new Entity();
			entity.AddComponent(new Rigidbody(entity));
			entity.AddComponent(new TestComponent(entity));

			// Act
			Engine.Instance.Initialize();
			Engine.CreateEntity(entity);
			entity.RemoveComponent(entity.Rigidbody!);

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
			//Assert.IsFalse(Engine.Instance.GetEngineComponent<IEventBus>()!.IsSubscribed(entity.GetComponent<TestComponent>()!));
			//Assert.IsFalse(Engine.Instance.GetEngineComponent<IEventBus>()!.IsSubscribed(entity.GetComponent<Rigidbody>()!));
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
			entity.Transform.Position = Vector3.One;
			entity.AddComponent(new TestComponent(entity));
			entity.AddComponent(new Rigidbody(entity));
			IEqualityComparer<Vector3> comparer = new Vector3EqualityComparer();

			// Act
			Engine.Instance.Initialize();
			Engine.CreateEntity(entity);
			var json = entity.SerializeToJson();
			var newEntity = Entity.DeserializeFromJson(json);

			// Assert
			Assert.IsNotNull(newEntity);
			Assert.IsNotNull(newEntity.GetComponent<TestComponent>());
			Assert.IsNotNull(newEntity.GetComponent<Rigidbody>());
			Assert.That(newEntity.Transform.Position, Is.EqualTo(entity.Transform.Position).Using(comparer));
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

		[Test]
		public void EntityDeserializeToJson_WhenMissingIdField_ShouldGenerateNew()
		{
			// Arrange
			var entity = new Entity();
			entity.Id = null!;

			// Act
			var json = entity.SerializeToJson();
			var newEntity = Entity.DeserializeFromJson(json);

			// Assert
			Assert.IsNotNull(newEntity.Id);
			Assert.That(newEntity.Id, Is.Not.EqualTo(entity.Id));
		}

		[Test]
		public void EntityDeserializeToJson_WhenMissingComponentType_ShouldIgnore()
		{
			// Arrange
			var entity = new Entity();
			entity.AddComponent(new TestComponent(entity));

			// Act
			var json = entity.SerializeToJson();
			var lines = json.Split('\n');
			lines = lines.Select(l => l.Contains("TestComponent") ? string.Empty : l).ToArray();
			var newJson = string.Join('\n', lines);
			var newEntity = Entity.DeserializeFromJson(newJson);

			// Assert
			Assert.IsNotNull(newEntity);
			Assert.IsNull(newEntity.GetComponent<TestComponent>());
		}

		[Test]
		public void EntityDeserializeToJson_WhenMissingComponentsArray_ShouldReturnEmptyObject()
		{
			// Arrange
			var entity = new Entity();
			entity.AddComponent(new TestComponent(entity));
			string pattern = @"""Components""\s*:\s*\[[^\]]*\],";

			// Act
			var json = entity.SerializeToJson();
			var newJson = Regex.Replace(json, pattern, "");
			var newEntity = Entity.DeserializeFromJson(newJson);

			// Assert
			Assert.IsNotNull(newEntity);
			Assert.IsNull(newEntity.GetComponent<TestComponent>());
			Assert.IsNotNull(newEntity.GetComponent<Transform>());
			Assert.IsNotNull(newEntity.Transform);
		}

		[Test]
		public void EntityDeserializeToJson_WhenMissingDataFromDTO_ShouldIgnore()
		{
			// Arrange
			var entity = new Entity();
			entity.AddComponent(new TestComponent(entity));
			string jsonString = @"
        {
          ""Components"": [
            {
              ""Type"": ""Basalt.Common.Components.Transform, Basalt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"",
              ""Data"": {
                ""IsFixedPoint"": false,
                ""Position"": {
                  ""X"": 0.0,
                  ""Y"": 0.0,
                  ""Z"": 0.0
                },
                ""Rotation"": {
                  ""X"": 0.0,
                  ""Y"": 0.0,
                  ""Z"": 0.0,
                  ""W"": 1.0,
                  ""IsIdentity"": true
                },
                ""Enabled"": true
              }
            },
			{
			  ""Type"": ""Basalt.Tests.Common.TestComponent, Basalt.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"",
			}
          ],
          ""Children"": [],
          ""Id"": null
        }";

			// Act
			var newEntity = Entity.DeserializeFromJson(jsonString);

			// Assert
			Assert.IsNotNull(newEntity);
			Assert.IsNull(newEntity.GetComponent<TestComponent>());
		}

		[Test]
		public void EntityEnabled_WhenDisabled_ShouldNotCallEvents()
		{
			// Arrange
			var entity = new Entity();
			entity.AddComponent(new TestComponent(entity));
			entity.Enabled = false;

			// Act
			var bus = Engine.Instance.GetEngineComponent<IEventBus>()!;
			Engine.Instance.Initialize();
			Engine.CreateEntity(entity);

			bus.NotifyStart();
			bus.NotifyRender();
			bus.NotifyPhysicsUpdate();
			bus.NotifyUpdate();

			// Assert
			Assert.That(entity.GetComponent<TestComponent>()!.OnUpdateCount, Is.EqualTo(0), "Update was called");
			Assert.That(entity.GetComponent<TestComponent>()!.OnRenderCount, Is.EqualTo(0), "Render was called");
			Assert.That(entity.GetComponent<TestComponent>()!.OnPhysicsUpdateCount, Is.EqualTo(0), "Physics Update was called");
		}
	}
}
