using Basalt.Utility;

namespace Basalt.Tests
{
	public class CircularBufferTests
	{
		[Test]
		public void CircularBuffer_Length_ReturnsSize()
		{
			// Arrange
			int size = 5;
			var buffer = new CircularBuffer<int>(size);

			// Act
			int length = buffer.Length;

			// Assert
			Assert.That(length, Is.EqualTo(size));
		}

		[Test]
		public void CircularBuffer_Indexer_Get_ReturnsValue()
		{
			// Arrange
			var buffer = new CircularBuffer<int>(5);
			buffer[0] = 10;
			buffer[1] = 20;

			// Act
			int value1 = buffer[0];
			int value2 = buffer[1];

			// Assert
			Assert.That(value1, Is.EqualTo(10));
			Assert.That(value2, Is.EqualTo(20));
		}

		[Test]
		public void CircularBuffer_Indexer_Set_SetsValue()
		{
			// Arrange
			var buffer = new CircularBuffer<int>(5);

			// Act
			buffer[0] = 10;
			buffer[1] = 20;

			// Assert
			Assert.That(buffer[0], Is.EqualTo(10));
			Assert.That(buffer[1], Is.EqualTo(20));
		}

		[Test]
		public void CircularBuffer_Next_ReturnsNextValue()
		{
			// Arrange
			var buffer = new CircularBuffer<int>(3);
			buffer[0] = 10;
			buffer[1] = 20;
			buffer[2] = 30;

			// Act
			int value1 = buffer.Next();
			int value2 = buffer.Next();
			int value3 = buffer.Next();

			// Assert
			Assert.That(value1, Is.EqualTo(10));
			Assert.That(value2, Is.EqualTo(20));
			Assert.That(value3, Is.EqualTo(30));
		}

		[Test]
		public void CircularBuffer_Next_WrapsAround()
		{
			// Arrange
			var buffer = new CircularBuffer<int>(3);
			buffer[0] = 10;
			buffer[1] = 20;
			buffer[2] = 30;

			// Act

			// Assert
			Assert.That(buffer.Next(), Is.EqualTo(10));
			Assert.That(buffer.Next(), Is.EqualTo(20));
			Assert.That(buffer.Next(), Is.EqualTo(30));
			Assert.That(buffer.Next(), Is.EqualTo(10));
			Assert.That(buffer.Next(), Is.EqualTo(20));
			Assert.That(buffer.Next(), Is.EqualTo(30));
		}

		[Test]
		public void CircularBuffer_Previous_WrapsAround()
		{
			// Arrange
			var buffer = new CircularBuffer<int>(3);
			buffer[0] = 10;
			buffer[1] = 20;
			buffer[2] = 30;

			// Act

			// Assert
			Assert.That(buffer.Previous(), Is.EqualTo(30));
			Assert.That(buffer.Previous(), Is.EqualTo(20));
			Assert.That(buffer.Previous(), Is.EqualTo(10));
			Assert.That(buffer.Previous(), Is.EqualTo(30));
			Assert.That(buffer.Previous(), Is.EqualTo(20));
			Assert.That(buffer.Previous(), Is.EqualTo(10));
		}

		[Test]
		public void CircularBuffer_Previous_ReturnsPreviousValue()
		{
			// Arrange
			var buffer = new CircularBuffer<int>(3);
			buffer[0] = 10;
			buffer[1] = 20;
			buffer[2] = 30;

			// Act

			// Assert
			Assert.That(buffer.Previous(), Is.EqualTo(30));
			Assert.That(buffer.Previous(), Is.EqualTo(20));
			Assert.That(buffer.Previous(), Is.EqualTo(10));
		}

		[Test]
		public void CircularBuffer_Clear_SetsAllValuesToDefault()
		{
			// Arrange
			var buffer = new CircularBuffer<int>(3);
			buffer[0] = 10;
			buffer[1] = 20;
			buffer[2] = 30;

			// Act
			buffer.Clear();

			// Assert
			Assert.That(buffer[0], Is.EqualTo(0));
			Assert.That(buffer[1], Is.EqualTo(0));
			Assert.That(buffer[2], Is.EqualTo(0));
		}

		[Test]
		public void CircularBuffer_FillWithValue_SetsAllValuesToGivenValue()
		{
			// Arrange
			var buffer = new CircularBuffer<int>(3);

			// Act
			buffer.Fill(10);

			// Assert
			Assert.That(buffer[0], Is.EqualTo(10));
			Assert.That(buffer[1], Is.EqualTo(10));
			Assert.That(buffer[2], Is.EqualTo(10));
		}

		[Test]
		public void CircularBuffer_FillWithArray_SetsValuesFromGivenArray()
		{
			// Arrange
			var buffer = new CircularBuffer<int>(4);
			int[] values = { 10, 20, 30 };

			// Act
			buffer.Fill(values);

			// Assert
			Assert.That(buffer[0], Is.EqualTo(10));
			Assert.That(buffer[1], Is.EqualTo(20));
			Assert.That(buffer[2], Is.EqualTo(30));
			Assert.That(buffer[3], Is.EqualTo(10));
		}

		[Test]
		public void CircularBuffer_FillWithArrayAndOffset_SetsValuesFromGivenArrayWithOffset()
		{
			// Arrange
			var buffer = new CircularBuffer<int>(4);
			int[] values = { 10, 20, 30 };

			// Act
			buffer.Fill(values, 1);

			// Assert
			Assert.That(buffer[0], Is.EqualTo(20));
			Assert.That(buffer[1], Is.EqualTo(30));
			Assert.That(buffer[2], Is.EqualTo(10));
			Assert.That(buffer[3], Is.EqualTo(20));
		}

		[Test]
		public void CircularBuffer_Constructor_WithBufferArray()
		{
			// Arrange
			int[] values = { 10, 20, 30 };

			// Act
			var buffer = new CircularBuffer<int>(values);

			// Assert
			Assert.That(buffer[0], Is.EqualTo(10));
			Assert.That(buffer[1], Is.EqualTo(20));
			Assert.That(buffer[2], Is.EqualTo(30));
			Assert.That(buffer.Next(), Is.EqualTo(10));
			Assert.That(buffer.Next(), Is.EqualTo(20));
			Assert.That(buffer.Next(), Is.EqualTo(30));
		}

		[Test]
		public void CircularBuffer_Constructor_WithBufferArrayAndIndex()
		{
			// Arrange
			int[] values = { 10, 20, 30 };

			// Act
			var buffer = new CircularBuffer<int>(values, 1);

			// Assert
			Assert.That(buffer.Next(), Is.EqualTo(20));
			Assert.That(buffer.Next(), Is.EqualTo(30));
			Assert.That(buffer.Next(), Is.EqualTo(10));
		}

		[Test]
		public void CircularBuffer_Constructor_WithGeneratorIntT()
		{
			// Arrange

			// Act
			var buffer = new CircularBuffer<int>(3, i => i * 2);

			// Assert
			Assert.That(buffer.Next(), Is.EqualTo(0));
			Assert.That(buffer.Next(), Is.EqualTo(2));
			Assert.That(buffer.Next(), Is.EqualTo(4));
		}

		[Test]
		public void CircularBuffer_Constructor_WithGeneratorT()
		{
			// Arrange

			// Act
			var buffer = new CircularBuffer<int>(3, () => 1);

			// Assert
			Assert.That(buffer.Next(), Is.EqualTo(1));
			Assert.That(buffer.Next(), Is.EqualTo(1));
			Assert.That(buffer.Next(), Is.EqualTo(1));
		}

	}
}
