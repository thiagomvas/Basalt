using Basalt.Core.Common.Abstractions.Engine;
using Basalt.Core.Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Core.Common.Abstractions.Input
{
    public interface IInputSystem : IEngineComponent
	{
		void Update();
		void RegisterKeybind(InputAction key, Action action);
		void ReplaceKeybind(InputAction oldKey, InputAction newKey);
	}
}
