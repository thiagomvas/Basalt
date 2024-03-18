using Basalt.Core.Common.Abstractions.Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Core.Common.Abstractions
{
	public interface IEngineBuilder
	{
		IEngineBuilder WithGraphicsEngine(IGraphicsEngine graphicsEngine);
		IEngineBuilder WithSoundEngine(ISoundSystem soundEngine);
		IEngineBuilder WithPhysicsEngine(IPhysicsEngine physicsEngine);
		IEngineBuilder WithLogger(ILogger logger);
		IEngine Build();
	}
}
