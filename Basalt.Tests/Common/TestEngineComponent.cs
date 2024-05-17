using Basalt.Core.Common.Abstractions.Engine;

namespace Basalt.Tests.Common
{
	public class TestEngineComponent : IEngineComponent
	{
		public int Value { get; set; } = -1;
		public bool Initialized { get; set; }
		public void Initialize()
		{
			Initialized = true;
		}

		public void Shutdown()
		{
			Initialized = false;
		}
	}
}
