using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yuhu.Infrastructure.Modules
{
    public class ModuleProvider : IModuleProvider
    {
        private readonly List<AbstractModule> _modules;
        private readonly CPlatformContainer _serviceProvoider;
        private readonly ILogger<ModuleProvider> _logger;

        public ModuleProvider(List<AbstractModule> modules,
            ILogger<ModuleProvider> logger,
            CPlatformContainer serviceProvoider)
        {
            _modules = modules;
            _serviceProvoider = serviceProvoider;
            _logger = logger;
        }

        public List<AbstractModule> Modules { get => _modules; }

        public void LoadModules()
        {
            foreach (var m in Modules)
            {
                //
            }
        }
    }
}
