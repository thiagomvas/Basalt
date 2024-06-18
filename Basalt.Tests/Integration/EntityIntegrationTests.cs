using Basalt.Common;
using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Common.Events;
using Basalt.Common.Utils;
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

			bus.TriggerEvent(BasaltConstants.StartEventKey);
			bus.TriggerEvent(BasaltConstants.RenderEventKey);
			bus.TriggerEvent(BasaltConstants.PhysicsUpdateEventKey);
			bus.TriggerEvent(BasaltConstants.StartEventKey);

			// Assert
			Assert.That(entity.GetComponent<TestComponent>()!.OnUpdateCount, Is.EqualTo(0), "Update was called");
			Assert.That(entity.GetComponent<TestComponent>()!.OnRenderCount, Is.EqualTo(0), "Render was called");
			Assert.That(entity.GetComponent<TestComponent>()!.OnPhysicsUpdateCount, Is.EqualTo(0), "Physics Update was called");
		}

		[Test]
		public void CloneEntity_ShouldReturnNewInstances()
		{
			// Arrange
			var entity = new Entity();
			entity.AddComponent(new TestComponent(entity));
			entity.AddComponent(new Rigidbody(entity));
			var tc = entity.GetComponent<TestComponent>()!;
			var rb = entity.GetComponent<Rigidbody>()!;

			// Act
			Engine.Instance.Initialize();
			Engine.CreateEntity(entity);
			var clone = entity.Clone();
			var ctc = clone.GetComponent<TestComponent>()!;
			var crb = clone.GetComponent<Rigidbody>()!;

			// Assert
			Assert.That(clone, Is.Not.Null);
			Assert.That(clone, Is.Not.EqualTo(entity));
			Assert.That(ctc, Is.Not.EqualTo(tc));
			Assert.That(crb, Is.Not.EqualTo(rb));
			Assert.That(ctc.Entity, Is.EqualTo(clone));
			Assert.That(crb.Entity, Is.EqualTo(clone));
		}

		[Test]
		public void CloneEntity_WhenCloning_ShouldCloneAllComponents()
		{
			// Arrange
			var entity = new Entity();
			entity.AddComponent(new TestComponent(entity));
			entity.AddComponent(new Rigidbody(entity));

			// Act
			Engine.Instance.Initialize();
			Engine.CreateEntity(entity);
			var clone = entity.Clone();

			// Assert
			Assert.IsNotNull(clone);
			Assert.IsNotNull(clone.GetComponent<TestComponent>());
			Assert.IsNotNull(clone.GetComponent<Rigidbody>());
		}


		[Test]
		public void CloneEntity_WhenCloning_ShouldCopyProperties()
		{
			// Arrange
			var entity = new Entity();
			entity.Transform.Position = Vector3.One;
			entity.AddComponent(new TestComponent(entity) { OnRenderCount = 42 });
			entity.AddComponent(new Rigidbody(entity) { Mass = 10, IsKinematic = true, Drag = 1.1f});

			var rb = entity.GetComponent<Rigidbody>()!;
			var tc = entity.GetComponent<TestComponent>()!;

			tc.Set(69);

			// Act
			Engine.Instance.Initialize();
			Engine.CreateEntity(entity);
			var clone = entity.Clone();
			var crb = clone.GetComponent<Rigidbody>()!;
			var ctc = clone.GetComponent<TestComponent>()!;

			// Assert
			Assert.That(clone.Transform.Position, Is.EqualTo(entity.Transform.Position).Using((IEqualityComparer<Vector3>) new Vector3EqualityComparer()));
			Assert.That(clone.GetComponent<TestComponent>()!.OnRenderCount, Is.EqualTo(entity.GetComponent<TestComponent>()!.OnRenderCount));
			Assert.That(crb.IsKinematic, Is.EqualTo(rb.IsKinematic));
			Assert.That(crb.Mass, Is.EqualTo(rb.Mass));
			Assert.That(crb.Drag, Is.EqualTo(rb.Drag));
			Assert.That(ctc.Foo, Is.Not.EqualTo(tc.Foo));
		}
	}
}
