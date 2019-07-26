using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yuhu.Infrastructure.Modules
{
    public abstract class AbstractModule : Autofac.Module, IDisposable
    {
        #region 实例属性

        public ContainerBuilderWrapper Builder { get; set; }

        public Guid Identifier { get; set; }


        public string ModuleName { get; set; }


        public string TypeName { get; set; }


        public string Title { get; set; }

        public bool Enable { get; set; } = true;


        public string Description { get; set; }


        #endregion

        public AbstractModule()
        {
            ModuleName = this.GetType().Name;
            TypeName = this.GetType().BaseType.FullName;
        }

      
        protected override void Load(ContainerBuilder builder)
        {
            try
            {
                base.Load(builder);
                Builder = new ContainerBuilderWrapper(builder);
                if (Enable)
                {
                    RegisterServices(Builder);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        //注册服务
        public virtual void RegisterServices(ContainerBuilderWrapper builder)
        {

        }

        public virtual void Dispose()
        {

        }

    }
}
