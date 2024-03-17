using Basalt.Core.Common.Abstractions;

namespace Basalt
{
	public interface IEngine
	{
		IEventBus? EventBus { get; }

		void Run();
		void Shutdown();
	}
}