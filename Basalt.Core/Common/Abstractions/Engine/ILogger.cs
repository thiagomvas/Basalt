using Basalt.Core.Common.Types;
using System.Runtime.CompilerServices;

namespace Basalt.Core.Common.Abstractions.Engine
{
    public interface ILogger
    {
        void Log(LogLevel level, string message, string callerName);
        void LogDebug(string message, [CallerMemberName] string callerName = "");
        void LogInformation(string message, [CallerMemberName] string callerName = "");
        void LogWarning(string message, [CallerMemberName] string callerName = "");
        void LogError(string message, [CallerMemberName] string callerName = "");
        void LogFatal(string message, [CallerMemberName] string callerName = "");
        void SaveLog(string path);
    }
}
