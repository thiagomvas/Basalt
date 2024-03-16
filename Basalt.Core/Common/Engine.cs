using Basalt.Core.Common.Abstractions;

namespace Basalt.Core.Common
{
	public class Engine
	{
		private readonly IGraphicsEngine? _graphicsEngine;
		private readonly ISoundSystem? _soundSystem;
		private readonly IPhysicsEngine? _physicsEngine;
		public ILogger? Logger;
		public Engine(IGraphicsEngine? graphicsEngine, ISoundSystem? soundSystem, IPhysicsEngine? physicsEngine)
		{
			_graphicsEngine = graphicsEngine;
			_soundSystem = soundSystem;
			_physicsEngine = physicsEngine;
		}

		public void Initialize()
		{
			Logger?.LogInformation("Engine Initializing");
			if(_graphicsEngine == null)
			{
				Logger?.LogFatal("Graphics engine not specified! Cannot run engine.");
				return;
			}
			if(_soundSystem == null)
			{
				Logger?.LogWarning("Sound system not specified! Engine will run without sound.");
			}

			if(_physicsEngine == null)
			{
				Logger?.LogWarning("Physics engine not specified! Engine will run without physics.");
			}

			_graphicsEngine?.Initialize();
			_soundSystem?.Initialize();
			_physicsEngine?.Initialize();
		}
	}
}
