using System.Collections;

namespace Basalt.Utility
{
	/// <summary>
	/// Represents a circular buffer data structure.
	/// </summary>
	/// <typeparam name="T">The type of elements stored in the buffer.</typeparam>
	/// <remarks>Works just like any collection, with a looping <see cref="Next"/> method that, once it reaches the end of the collection, the index loops back to 0. Same for <see cref="Previous"/></remarks>
	public class CircularBuffer<T> : IEnumerable<T>
	{
		private readonly T[] _buffer;
		private readonly int _size;
		private int _index = 0;

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="CircularBuffer{T}"/> class with the specified size.
		/// </summary>
		/// <param name="size">The size of the buffer.</param>
		public CircularBuffer(int size)
		{
			_buffer = new T[size];
			_index = 0;
			_size = size;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CircularBuffer{T}"/> class with the specified buffer and index.
		/// </summary>
		/// <param name="buffer">The buffer to use.</param>
		/// <param name="index">The index to start from.</param>
		public CircularBuffer(T[] buffer, int index)
		{
			_buffer = buffer;
			_size = buffer.Length;
			_index = index % _size;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CircularBuffer{T}"/> class with the specified buffer.
		/// </summary>
		/// <param name="buffer">The buffer to use.</param>
		public CircularBuffer(T[] buffer)
		{
			_buffer = buffer;
			_index = 0;
			_size = buffer.Length;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CircularBuffer{T}"/> class with the specified size and generator function.
		/// </summary>
		/// <param name="size">The size of the buffer.</param>
		/// <param name="generator">The generator function with an index parameter to use for initializing the buffer elements. </param>
		public CircularBuffer(int size, Func<int, T> generator)
		{
			_buffer = new T[size];
			_size = size;
			for (int i = 0; i < size; i++)
			{
				_buffer[i] = generator(i);
			}
			_index = 0;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="CircularBuffer{T}"/> class with the specified size and generator function.
		/// </summary>
		/// <param name="size">The size of the buffer.</param>
		/// <param name="generator">The generator function to use for initializing the buffer elements. </param>
		public CircularBuffer(int size, Func<T> generator)
		{
			_buffer = new T[size];
			_size = size;
			for (int i = 0; i < size; i++)
			{
				_buffer[i] = generator();
			}
			_index = 0;
		}
		
		#endregion

		/// <summary>
		/// Gets the length of the buffer.
		/// </summary>
		public int Length => _size;

		/// <summary>
		/// Gets the current index of the buffer.
		/// </summary>
		public int Index => _index;

		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		/// <param name="index">The index of the element to get or set.</param>
		/// <returns>The element at the specified index.</returns>
		public T this[int index]
		{
			get => _buffer[index];
			set => _buffer[index] = value;
		}

		/// <summary>
		/// Returns the element at the current index and moves the index to the next element in the buffer.
		/// </summary>
		/// <returns>The element at the current index.</returns>
		public T Next()
		{
			var result = _buffer[_index];
			_index = (_index + 1) % _buffer.Length;
			return result;
		}

		/// <summary>
		/// Moves the index to the previous element in the buffer and returns it.
		/// </summary>
		/// <returns>The previous element in the buffer.</returns>
		public T Previous()
		{
			_index = (_index - 1 + _buffer.Length) % _buffer.Length;
			return _buffer[_index];
		}

		/// <summary>
		/// Clears the buffer by setting all elements to their default value.
		/// </summary>
		public void Clear()
		{
			for (int i = 0; i < _buffer.Length; i++)
			{
				_buffer[i] = default;
			}
		}

		/// <summary>
		/// Fills the buffer with the specified value.
		/// </summary>
		/// <param name="value">The value to fill the buffer with.</param>
		public void Fill(T value)
		{
			for (int i = 0; i < _buffer.Length; i++)
			{
				_buffer[i] = value;
			}
		}

		/// <summary>
		/// Fills the buffer with the specified array of values, repeating them if necessary.
		/// </summary>
		/// <param name="values">The array of values to fill the buffer with.</param>
		public void Fill(T[] values)
		{
			for (int i = 0; i < _buffer.Length; i++)
			{
				_buffer[i] = values[i % values.Length];
			}
		}

		/// <summary>
		/// Fills the buffer with the specified array of values, starting from the specified offset and repeating them if necessary.
		/// </summary>
		/// <param name="values">The array of values to fill the buffer with.</param>
		/// <param name="offset">The offset to start filling the buffer from.</param>
		public void Fill(T[] values, int offset)
		{
			for (int i = 0; i < _buffer.Length; i++)
			{
				_buffer[i] = values[(i + offset) % values.Length];
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			return ((IEnumerable<T>)_buffer).GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
