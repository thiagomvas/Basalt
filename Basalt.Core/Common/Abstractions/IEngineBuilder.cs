using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Core.Common.Abstractions
{
	public interface IEngineBuilder
	{
		IEngineBuilder UseGraphicsEngine(IGraphicsEngine graphicsEngine);
		IEngineBuilder UseSoundEngine(ISoundSystem soundEngine);
		IEngineBuilder UsePhysicsEngine(IPhysicsEngine physicsEngine);
		Engine Build();
	}
}
