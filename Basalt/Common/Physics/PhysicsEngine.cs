using Basalt.Core.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Common.Physics
{
	public class PhysicsEngine : IPhysicsEngine
	{
		private readonly ILogger? logger;
		private bool ShouldRun = true;

		public PhysicsEngine(ILogger? logger = null)
		{
			this.logger = logger;
		}
		public void Initialize()
		{
			logger?.LogInformation("Physics Engine Initialized");
			while(ShouldRun)
			{
			}
			logger?.LogWarning("Shut down physics engine");
		}

		public void Shutdown()
		{
			ShouldRun = false;
			logger?.LogWarning("Shutting down physics engine...");
		}
	}
}
