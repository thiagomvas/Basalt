using Basalt.Common.Entities;
using Basalt.Tests.Common;
using System.Numerics;

namespace Basalt.Tests.Unit.Components
{
	[TestFixture]
	internal class TransformUnitTests
	{
		[Test]
		public void TransformPositionSet_WhenFixedPointIsTrue_ShouldNotChangePosition()
		{
			// Arrange
			var entity = new Entity();
			entity.Transform.IsFixedPoint = true;
			var initialPosition = entity.Transform.Position;
			var newPosition = new Vector3(1, 2, 3);

			// Act
			entity.Transform.Position = newPosition;

			// Assert
			Assert.That(entity.Transform.Position, Is.EqualTo(initialPosition));
		}

		[Test]
		public void TransformPositionSet_WhenHasChildren_ShouldUpdateChildrenPositions()
		{
			// Arrange
			var parent = new Entity();
			var child1 = new Entity();
			var child2 = new Entity();
			parent.AddChildren(child1);
			parent.AddChildren(child2);

			var initialPosition = parent.Transform.Position;
			var newPosition = new Vector3(1, 2, 3);

			// Act
			parent.Transform.Position = newPosition;

			// Assert
			Assert.That(parent.Transform.Position, Is.EqualTo(newPosition));
			Assert.That(child1.Transform.Position, Is.EqualTo(newPosition));
			Assert.That(child2.Transform.Position, Is.EqualTo(newPosition));
		}

		[Test]
		public void TransformRotationSet_ShouldUpdateForwardRightUp()
		{
			// Arrange
			var entity = new Entity();
			var newRotation = Quaternion.CreateFromYawPitchRoll(1, 2, 3);
			IEqualityComparer<Vector3> vector3Comparer = new Vector3EqualityComparer();

			// Act
			entity.Transform.Rotation = newRotation;
			var expectedForward = Vector3.Transform(Vector3.UnitZ, newRotation);
			var expectedRight = Vector3.Transform(-Vector3.UnitX, newRotation);
			var expectedUp = Vector3.Transform(-Vector3.UnitY, newRotation);

			// Assert
			Assert.That(entity.Transform.Rotation, Is.EqualTo(newRotation));
			Assert.That(entity.Transform.Forward, Is.EqualTo(expectedForward).Using(vector3Comparer), "Forward is wrong");
			Assert.That(entity.Transform.Right, Is.EqualTo(expectedRight).Using(vector3Comparer), "Right is wrong");
			Assert.That(entity.Transform.Up, Is.EqualTo(expectedUp).Using(vector3Comparer), "Up is wrong");
		}

		[Test]
		public void TransformRotationSet_WhenHasChildren_ShouldUpdateChildrenRotation()
		{
			// Arrange
			var parent = new Entity();
			var child1 = new Entity();
			var child2 = new Entity();
			parent.AddChildren(child1);
			parent.AddChildren(child2);

			var newRotation = Quaternion.CreateFromYawPitchRoll(1, 2, 3);

			// Act
			parent.Transform.Rotation = newRotation;

			// Assert
			Assert.That(parent.Transform.Rotation, Is.EqualTo(newRotation));
			Assert.That(child1.Transform.Rotation, Is.EqualTo(newRotation));
			Assert.That(child2.Transform.Rotation, Is.EqualTo(newRotation));
		}
	}
}
