using Basalt.Core.Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Core.Common.Abstractions.Input
{
	public struct InputAction
	{
		public InputKey Key { get; set; }
		public ActionType Type { get; set; }

		public InputAction(InputKey key, ActionType type)
		{
			Key = key;
			Type = type;
		}

	}
}
