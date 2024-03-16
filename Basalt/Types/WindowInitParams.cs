﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Types
{
	public class WindowInitParams
	{
		public string Title { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public int TargetFps { get; set; }
		public bool Fullscreen { get; set; }

		public bool VSync { get; set; }
	}
}
