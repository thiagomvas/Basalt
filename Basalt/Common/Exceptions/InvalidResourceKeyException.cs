namespace Basalt.Common.Exceptions
{
	public class InvalidResourceKeyException : Exception
	{
		public InvalidResourceKeyException(string paramName, string paramValue) 
			: base($"The resource key '{paramValue}' for '{paramName}' is invalid or not found. Make sure the key is correctly typed or whether the resource was loaded or not.")
		{
		}
	}
}
