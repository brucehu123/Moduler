using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Yuhu.Infrastructure.Utilities;

namespace Yuhu.Infrastructure.Dependency
{
    public class AutofacBootstrap
    {
        public IServiceProvider Initialize(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            //todo: 在这里注册服务组件

            builder.Populate(services);

            builder.ConfigureServices(option =>
            {
                option.RegisterModules();
            });

            ServiceLocator.Current = builder.Build();
            builder.Register(m => new CPlatformContainer(ServiceLocator.Current));
            return new AutofacServiceProvider(ServiceLocator.Current);
        }
    }
}
