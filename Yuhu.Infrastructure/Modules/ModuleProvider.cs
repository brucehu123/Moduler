using System;
using System.Collections.Generic;
using System.Text;

namespace Yuhu.Infrastructure.Modules
{
    public class ModuleProvider : IModuleProvider
    {
        public List<AbstractModule> Modules { get; }

        public void LoadModules()
        {
            foreach (var m in Modules)
            {
                //
            }
        }
    }
}
