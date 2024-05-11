using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basalt.Core.Common.Abstractions.Engine
{
    public interface IGraphicsEngine : IEngineComponent
    {
        void Render();

    }
}
