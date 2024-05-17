namespace Basalt.Math
{

	///<summary>
	/// Represents a Perlin noise generator.
	/// </summary>
	/// <remarks>
	/// Using rounded integers for parameter will result in a repeating constant value. 
	/// For usage, make sure to rescale the input values to avoid this. Best used with floating point values ranging from 0 to 1. The smaller the variation, the better. 
	/// See example below
	/// <code>
	/// var noise = new PerlinNoise(123, 8);
	/// int width = 16, height = 16;
	/// float[,] noiseMap = new float[width, height];
	/// float xOffset = 0;
	/// float yOffset = 0;
	/// for (int x = 0; x &lt; width; x++)
	/// {
	/// 	for (int y = 0; y &lt; height; y++)
	/// 	{
	/// 		float xCoord = (float)x / width + xOffset;
	/// 		float yCoord = (float)y / height + yOffset;
	/// 		var val = noise.Generate(xCoord, yCoord);
	/// 		noiseMap[x, y] = val;
	/// 	}
	/// }
	/// </code>
	/// </remarks>
	public class PerlinNoise
	{
		private readonly int[] permutation;
		private const int permutationSize = 256;
		private readonly int octaves;

		private float localMin = -0.5f, localMax = 0.5f;
		public float Min { get; private set; } = float.NaN;
		public float Max { get; private set; } = float.NaN;

		bool assignedminmax = false;

		/// <summary>
		/// Initializes a new seeded instance of the <see cref="PerlinNoise"/> class with the specified seed and number of octaves.
		/// </summary>
		/// <param name="seed">The seed value for random number generation.</param>
		/// <param name="octaves">The number of octaves to use in generating the Perlin noise.</param>
		public PerlinNoise(int seed, int octaves)
		{
			permutation = new int[permutationSize];

			var rnd = new Random(seed);
			for (int i = 0; i < permutationSize; i++)
			{
				permutation[i] = rnd.Next(0, permutationSize);
			}

			this.octaves = octaves;
		}

		public PerlinNoise(int seed, int octaves, float min, float max)
		{
			permutation = new int[permutationSize];

			var rnd = new Random(seed);
			for (int i = 0; i < permutationSize; i++)
			{
				permutation[i] = rnd.Next(0, permutationSize);
			}
			this.octaves = octaves;

			if (max < min)
				(min, max) = (max, min); // swap values if min is greater than max (for user convenience)

			Min = min;
			Max = max;
			assignedminmax = true;
		}

		/// <summary>
		/// Performs fade interpolation.
		/// </summary>
		/// <param name="t">The input value.</param>
		/// <returns>The result of the fade interpolation.</returns>
		private float Fade(float t)
		{
			return t * t * t * (t * (t * 6 - 15) + 10);
		}

		/// <summary>
		/// Computes the gradient value.
		/// </summary>
		/// <param name="hash">The hash value.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The gradient value.</returns>
		private float Grad(int hash, float x, float y)
		{
			int h = hash & 15;
			float u = h < 8 ? x : y;
			float v = h < 4 ? y : x;
			return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
		}

		/// <summary>
		/// Computes 2D Perlin noise.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The Perlin noise value at the given coordinates.</returns>
		private float PerlinNoise2D(float x, float y)
		{
			// Find the integer coordinates of the cell containing the point (x, y)
			int X = (int)System.Math.Floor(x) & 255;
			int Y = (int)System.Math.Floor(y) & 255;

			// Find the fractional part of the coordinates
			x -= System.MathF.Floor(x);
			y -= System.MathF.Floor(y);

			// Compute the fade values for interpolation
			float u = Fade(x);
			float v = Fade(y);

			// Hash coordinates of the 4 corners of the cell
			int A = permutation[X] + Y;
			int AA = permutation[A % permutationSize];
			int AB = permutation[(A + 1) % permutationSize];
			int B = permutation[(X + 1) % permutationSize] + Y;
			int BA = permutation[B % permutationSize];
			int BB = permutation[(B + 1) % permutationSize];

			// I don't know how this works, but it does and that's all that matters.
			// Interpolate the gradients
			return Lerp(v, Lerp(u, Grad(permutation[AA], x, y),
Grad(permutation[BA], x - 1, y)),
Lerp(u, Grad(permutation[AB], x, y - 1),
Grad(permutation[BB], x - 1, y - 1)));
		}

		/// <summary>
		/// Performs linear interpolation.
		/// </summary>
		/// <param name="t">The interpolation factor.</param>
		/// <param name="a">The start value.</param>
		/// <param name="b">The end value.</param>
		/// <returns>The result of the linear interpolation.</returns>
		private float Lerp(float t, float a, float b)
		{
			return a + t * (b - a);
		}

		/// <summary>
		/// Generates Perlin noise at the specified coordinates.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The Perlin noise value at the given coordinates.</returns>
		public float Generate(float x, float y)
		{
			float total = 0;
			float frequency = 1;
			float amplitude = 1;
			float maxValue = 0;

			for (int i = 0; i < octaves; i++)
			{
				total += PerlinNoise2D(x * frequency, y * frequency) * amplitude;
				maxValue += amplitude;
				amplitude *= 0.5f;
				frequency *= 2;
			}

			var result = total / maxValue;
			if (result < localMin)
			{
				localMin = result;
			}

			if (result > localMax)
			{
				localMax = result;
			}

			if (assignedminmax)
			{
				return BasaltMath.Scale(result, localMin, localMax, Min, Max);
			}

			return result;
		}
	}

}
