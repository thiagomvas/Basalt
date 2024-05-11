using Basalt.Core.Common.Abstractions.Engine;
using Basalt.Core.Common.Types;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Basalt.Common.Logging
{
    /// <summary>
    /// Represents a console logger implementation that logs messages to the console.
    /// </summary>
    public class ConsoleLogger : ILogger
	{
		private List<string> logLines = new List<string>();
		private LogLevel logLevel;

		/// <summary>
		/// Logs a message with the specified log level.
		/// </summary>
		/// <param name="level">The log level.</param>
		/// <param name="message">The message to log.</param>
		/// <param name="callerName">The name of the calling method.</param>
		public void Log(LogLevel level, string message, string callerName = "")
		{
			var frame = new StackTrace(1).GetFrame(1);
			string levelString = "";
			if (logLevel == LogLevel.Debug)
				levelString = $"[{level} : {new StackTrace(1).GetFrame(1)?.GetMethod()?.DeclaringType.Name}.{callerName}]";
			else
				levelString = $"[{level}]";

			string log = $"{levelString} [{DateTime.Now}]  {message}";
			logLines.Add(log);

			if (level >= logLevel)
			{
				SetAppropriateColor(level);
				Console.WriteLine(log);
				Console.ResetColor();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ConsoleLogger"/> class with the default log level set to <see cref="LogLevel.Debug"/>.
		/// </summary>
		public ConsoleLogger()
		{
			logLevel = LogLevel.Debug;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ConsoleLogger"/> class with the specified log level.
		/// </summary>
		/// <param name="logLevel">The log level.</param>
		public ConsoleLogger(LogLevel logLevel)
		{
			this.logLevel = logLevel;
		}

		private void SetAppropriateColor(LogLevel level)
		{
			switch (level)
			{
				case LogLevel.Debug:
					Console.ForegroundColor = ConsoleColor.DarkGray;
					break;
				case LogLevel.Info:
					Console.ForegroundColor = ConsoleColor.Gray;
					break;
				case LogLevel.Warning:
					Console.ForegroundColor = ConsoleColor.Yellow;
					break;
				case LogLevel.Error:
					Console.ForegroundColor = ConsoleColor.Red;
					break;
				case LogLevel.Fatal:
					Console.ForegroundColor = ConsoleColor.DarkRed;
					break;
				default:
					Console.ForegroundColor = ConsoleColor.White;
					break;
			}
		}

		/// <summary>
		/// Logs a debug message.
		/// </summary>
		/// <param name="message">The debug message to log.</param>
		/// <param name="callerName">The name of the calling method.</param>
		public void LogDebug(string message, [CallerMemberName] string callerName = "")
		{
			Log(LogLevel.Debug, message, callerName);
		}

		/// <summary>
		/// Logs an information message.
		/// </summary>
		/// <param name="message">The information message to log.</param>
		/// <param name="callerName">The name of the calling method.</param>
		public void LogInformation(string message, [CallerMemberName] string callerName = "")
		{
			Log(LogLevel.Info, message, callerName);
		}

		/// <summary>
		/// Logs a warning message.
		/// </summary>
		/// <param name="message">The warning message to log.</param>
		/// <param name="callerName">The name of the calling method.</param>
		public void LogWarning(string message, [CallerMemberName] string callerName = "")
		{
			Log(LogLevel.Warning, message, callerName);
		}

		/// <summary>
		/// Logs an error message.
		/// </summary>
		/// <param name="message">The error message to log.</param>
		/// <param name="callerName">The name of the calling method.</param>
		public void LogError(string message, [CallerMemberName] string callerName = "")
		{
			Log(LogLevel.Error, message, callerName);
		}

		/// <summary>
		/// Logs a fatal error message.
		/// </summary>
		/// <param name="message">The fatal error message to log.</param>
		/// <param name="callerName">The name of the calling method.</param>
		public void LogFatal(string message, [CallerMemberName] string callerName = "")
		{
			Log(LogLevel.Fatal, message, callerName);
		}

		public void SaveLog(string path)
		{
			if (!Directory.Exists("crash_reports"))
			{
				Directory.CreateDirectory("crash_reports");
			}
			File.WriteAllLines(Path.Combine("crash_reports", path), logLines);
		}
	}
}
