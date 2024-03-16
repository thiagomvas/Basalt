using Basalt.Core.Common.Abstractions;
using Basalt.Core.Common.Types;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace Basalt.Common.Logging
{
	public class ConsoleLogger : ILogger
	{
		private LogLevel logLevel;

		public void Log(LogLevel level, string message)
		{
			if (level >= logLevel)
			{
				SetAppropriateColor(level);
				string levelString = level == LogLevel.Debug ? $"[{level}]" : $"[{level} : {new StackTrace(1).GetFrame(1)?.GetMethod()?.DeclaringType.Name}]";
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


		public void LogDebug(string message)
		{
			Log(LogLevel.Debug, message);
		}

		public void LogInformation(string message)
		{
			Log(LogLevel.Info, message);
		}

		public void LogWarning(string message)
		{
			Log(LogLevel.Warning, message);
		}

		public void LogError(string message)
		{
			Log(LogLevel.Error, message);
		}

		public void LogFatal(string message)
		{
			Log(LogLevel.Fatal, message);
		}
	}
}
