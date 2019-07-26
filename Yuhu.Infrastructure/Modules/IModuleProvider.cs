using System;
using System.Collections.Generic;
using System.Text;

namespace Yuhu.Infrastructure.Modules
{
    public interface IModuleProvider
    {
        List<AbstractModule> Modules { get; }

        void LoadModules();
    }
}
