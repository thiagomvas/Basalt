using Basalt.Core.Common.Types;
using System.Runtime.CompilerServices;

namespace Basalt.Core.Common.Abstractions.Engine
{
	/// <summary>
	/// Represents a logger interface for logging messages with different log levels.
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		/// Logs a message with the specified log level.
		/// </summary>
		/// <param name="level">The log level.</param>
		/// <param name="message">The message to be logged.</param>
		/// <param name="callerName">The name of the calling member.</param>
		void Log(LogLevel level, string message, string callerName);

		/// <summary>
		/// Logs a debug message.
		/// </summary>
		/// <param name="message">The debug message to be logged.</param>
		/// <param name="callerName">The name of the calling member.</param>
		void LogDebug(string message, [CallerMemberName] string callerName = "");

		/// <summary>
		/// Logs an information message.
		/// </summary>
		/// <param name="message">The information message to be logged.</param>
		/// <param name="callerName">The name of the calling member.</param>
		void LogInformation(string message, [CallerMemberName] string callerName = "");

		/// <summary>
		/// Logs a warning message.
		/// </summary>
		/// <param name="message">The warning message to be logged.</param>
		/// <param name="callerName">The name of the calling member.</param>
		void LogWarning(string message, [CallerMemberName] string callerName = "");

		/// <summary>
		/// Logs an error message.
		/// </summary>
		/// <param name="message">The error message to be logged.</param>
		/// <param name="callerName">The name of the calling member.</param>
		void LogError(string message, [CallerMemberName] string callerName = "");

		/// <summary>
		/// Logs a fatal error message.
		/// </summary>
		/// <param name="message">The fatal error message to be logged.</param>
		/// <param name="callerName">The name of the calling member.</param>
		void LogFatal(string message, [CallerMemberName] string callerName = "");

		/// <summary>
		/// Saves the log to the specified path.
		/// </summary>
		/// <param name="path">The path where the log should be saved.</param>
		void SaveLog(string path);
	}
}
