using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Raylib.Components;
using Raylib_cs;
using System.Numerics;

namespace Basalt.TestField
{
	public static class TestingUtils
	{
		public static void SetupTestingScene(int boxCount = 10, int ropeLength = 20)
		{
			var ground = new Entity();
			ground.Transform.Position = new Vector3(0, -1, 0);
			ground.AddComponent(new BoxRenderer(ground) { Size = new Vector3(60, 1, 60), Color = Color.Gray });
			ground.AddComponent(new BoxCollider(ground) { Size = new Vector3(60, 1, 60) });
			ground.AddComponent(new Rigidbody(ground) { IsKinematic = true });
			ground.Id = "ground";
			Engine.CreateEntity(ground);

			// Instantiate boxes randomly on top of grund
			for (int i = 0; i < boxCount; i++)
			{
				var box = new Entity();
				box.Transform.Position = new Vector3(Random.Shared.Next(-30, 30), 25 + i, Random.Shared.Next(-30, 30));
				box.AddComponent(new ModelRenderer(box) { ModelCacheKey = "robot", Size = new Vector3((float)(i + 1) / 5f), Offset = -Vector3.UnitY * (float)(i + 1) / 2.5f });
				box.AddComponent(new BoxCollider(box) { Size = new Vector3(i) });
				box.AddComponent(new Rigidbody(box) { IsKinematic = false, Mass = 1 });

				Engine.CreateEntity(box);
			}

			List<Entity> links = new();
			// Horizontal rope
			for (int i = 0; i < ropeLength; i++)
			{
				var link = new Entity();
				links.Add(link);
				link.Transform.Position = new Vector3(20, 10, i * 2 - ropeLength);
				link.AddComponent(new BoxRenderer(link) { Size = new Vector3(1, 1, 1), Color = Color.Green });
				link.AddComponent(new BoxCollider(link) { Size = new Vector3(1, 1, 1) });
				link.AddComponent(new Rigidbody(link) { IsKinematic = false, Mass = 1 });
				if (i > 0)
				{
					link.AddComponent(new FixedLink(link) { AnchoredEntity = links[i - 1] });
					link.AddComponent(new EntityLineRenderer(link) { Color = Color.Green, Target = links[i - 1] });
					link.GetComponent<EntityLineRenderer>()!.SetRadius(0.25f);
				}
				if (i == 0 || i == ropeLength - 1)
				{
					link.Transform.IsFixedPoint = true;
					link.Rigidbody!.IsKinematic = true;
				}

				Engine.CreateEntity(link);
			}

		}
	}
}
