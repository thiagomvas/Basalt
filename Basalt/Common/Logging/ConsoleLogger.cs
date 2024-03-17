using Basalt.Core.Common.Abstractions;
using Basalt.Core.Common.Types;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Basalt.Common.Logging
{
	public class ConsoleLogger : ILogger
	{
		private LogLevel logLevel;

		public void Log(LogLevel level, string message, string callerName = "")
		{
			var frame = new StackTrace(1).GetFrame(1);
			if (level >= logLevel)
			{
				SetAppropriateColor(level);
				string levelString = "";
				if (logLevel == LogLevel.Debug)
					levelString = $"[{level} : {new StackTrace(1).GetFrame(1)?.GetMethod()?.DeclaringType.Name}.{callerName}]";
				else
					levelString = $"[{level}]";
				Console.WriteLine($"{levelString} [{DateTime.Now}]  {message}");
				Console.ResetColor();
			}
		}

		public ConsoleLogger()
		{
			logLevel = LogLevel.Debug;
		}

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


		public void LogDebug(string message, [CallerMemberName] string callerName = "")
		{
			Log(LogLevel.Debug, message, callerName);
		}

		public void LogInformation(string message, [CallerMemberName] string callerName = "")
		{
			Log(LogLevel.Info, message, callerName);
		}

		public void LogWarning(string message, [CallerMemberName] string callerName = "")
		{
			Log(LogLevel.Warning, message , callerName);
		}

		public void LogError(string message, [CallerMemberName] string callerName = "")
		{
			Log(LogLevel.Error, message , callerName);
		}

		public void LogFatal(string message, [CallerMemberName] string callerName = "")
		{
			Log(LogLevel.Fatal, message , callerName);
		}
	}
}
