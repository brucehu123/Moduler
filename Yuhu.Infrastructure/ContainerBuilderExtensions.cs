using Autofac;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Yuhu.Infrastructure.Modules;

namespace Yuhu.Infrastructure
{
    /// <summary>
    /// 服务构建者。
    /// </summary>
    public interface IServiceBuilder
    {
        /// <summary>
        /// 服务集合。
        /// </summary>
        ContainerBuilder Services { get; set; }
    }

    /// <summary>
    /// 默认服务构建者。
    /// </summary>
    internal sealed class ServiceBuilder : IServiceBuilder
    {
        public ServiceBuilder(ContainerBuilder services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            Services = services;
        }

        #region Implementation of IServiceBuilder

        /// <summary>
        /// 服务集合。
        /// </summary>
        public ContainerBuilder Services { get; set; }

        #endregion Implementation of IServiceBuilder
    }
    public static class ContainerBuilderExtensions
    {
        private static List<Assembly> _referenceAssembly = new List<Assembly>();
        private static List<AbstractModule> _modules = new List<AbstractModule>();

        public static void ConfigureServices(this ContainerBuilder builder, Action<IServiceBuilder> option)
        {
            option?.Invoke(builder.RegisterCore());
        }

        public static IServiceBuilder RegisterCore(this ContainerBuilder builder)
        {
            return new ServiceBuilder(builder);
        }

        public static IServiceBuilder RegisterModules(this IServiceBuilder builder)
        {
            var services = builder.Services;
            if (builder == null) throw new ArgumentNullException("builder");
            #region MyRegion
            //var packages = ConvertDictionary(AppConfig.ServerOptions.Packages);
            //foreach (var moduleAssembly in referenceAssemblies)
            //{
            //    GetAbstractModules(moduleAssembly).ForEach(p =>
            //    {
            //        services.RegisterModule(p);
            //        if (packages.ContainsKey(p.TypeName))
            //        {
            //            var useModules = packages[p.TypeName];
            //            if (useModules.AsSpan().IndexOf(p.ModuleName) >= 0)
            //                p.Enable = true;
            //            else
            //                p.Enable = false;
            //        }
            //        _modules.Add(p);
            //    });
            //}
            //builder.Services.Register(provider => new ModuleProvider(
            //   _modules, virtualPaths, provider.Resolve<ILogger<ModuleProvider>>(), provider.Resolve<CPlatformContainer>()
            //    )).As<IModuleProvider>().SingleInstance();

            //builder.Services.Register(provider => new ModuleProvider()).As<IModuleProvider>().SingleInstance();
            #endregion
            return builder;
        }
    }
}
