using Autofac;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
            if (builder == null) throw new ArgumentNullException("builder");
            var services = builder.Services;

            //todo: 读取配置文件
            //todo: 加载List<AbstractModule>()
            //todo: 注入IModuleProvider
            var referenceAssemblies = GetAssemblies();
            foreach (var moduleAssembly in referenceAssemblies)
            {
                GetAbstractModules(moduleAssembly).ForEach(s =>
                {
                    services.RegisterModule(s);

                    //todo:从配置文件里确定module是否启用

                    _modules.Add(s);
                });
            }
            services.Register(provider => new ModuleProvider(
                _modules, provider.Resolve<ILogger<ModuleProvider>>(), provider.Resolve<CPlatformContainer>()
                )).As<IModuleProvider>().SingleInstance();
            return builder;
        }


        private static List<Assembly> GetAssemblies()
        {
            var referenceAssemblies = new List<Assembly>();
            string[] assemblyNames = DependencyContext
                    .Default.GetDefaultAssemblyNames().Select(p => p.Name).ToArray();
            assemblyNames = GetFilterAssemblies(assemblyNames);
            foreach (var name in assemblyNames)
                referenceAssemblies.Add(Assembly.Load(name));
            _referenceAssembly.AddRange(referenceAssemblies.Except(_referenceAssembly));

            return referenceAssemblies;
        }

        private static List<AbstractModule> GetAbstractModules(Assembly assembly)
        {
            var abstractModules = new List<AbstractModule>();
            Type[] arrayModule =
                assembly.GetTypes().Where(
                    t => t.IsSubclassOf(typeof(AbstractModule))).ToArray();
            foreach (var moduleType in arrayModule)
            {
                var abstractModule = (AbstractModule)Activator.CreateInstance(moduleType);
                abstractModules.Add(abstractModule);
            }
            return abstractModules;
        }


        private static string[] GetFilterAssemblies(string[] assemblyNames)
        {
            //var notRelatedFile = AppConfig.ServerOptions.NotRelatedAssemblyFiles;
            //var relatedFile = AppConfig.ServerOptions.RelatedAssemblyFiles;
            string notRelatedFile = null;
            var relatedFile = "";
            var pattern = string.Format("^Microsoft.\\w*|^System.\\w*|^DotNetty.\\w*|^runtime.\\w*|^ZooKeeperNetEx\\w*|^StackExchange.Redis\\w*|^Consul\\w*|^Newtonsoft.Json.\\w*|^Autofac.\\w*{0}",
               string.IsNullOrEmpty(notRelatedFile) ? "" : $"|{notRelatedFile}");
            Regex notRelatedRegex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex relatedRegex = new Regex(relatedFile, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (!string.IsNullOrEmpty(relatedFile))
            {
                return
                    assemblyNames.Where(
                        name => !notRelatedRegex.IsMatch(name) && relatedRegex.IsMatch(name)).ToArray();
            }
            else
            {
                return
                    assemblyNames.Where(
                        name => !notRelatedRegex.IsMatch(name)).ToArray();
            }
        }
    }
}
