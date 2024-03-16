using Basalt.Core.Common.Types;

namespace Basalt.Core.Common.Abstractions
{
	public interface ILogger
	{
		void Log(LogLevel level, string message);
		void LogDebug(string message);
		void LogInformation(string message);
		void LogWarning(string message);
		void LogError(string message);
		void LogFatal(string message);
	}
}
