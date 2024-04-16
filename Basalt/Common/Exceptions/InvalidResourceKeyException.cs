namespace Basalt.Common.Exceptions
{
	public class InvalidResourceKeyException : Exception
	{
		public InvalidResourceKeyException(string key) : base($"The resource key '{key}' is invalid or not found. Make sure the key is correctly typed or whether the resource was loaded or not.")
		{
		}
	}
}
