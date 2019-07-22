using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using Yuhu.Infrastructure.Utilities;

namespace Yuhu.Infrastructure.Dependency
{
    public class ServiceResolver : IDependencyResolver
    {

        #region 字段
        private static readonly ServiceResolver _defaultInstance = new ServiceResolver();
        private readonly ConcurrentDictionary<ValueTuple<Type, string>, object> _initializers =
            new ConcurrentDictionary<ValueTuple<Type, string>, object>();
        #endregion

        #region 公共方法
        /// <summary>
        /// 注册对象添加到IOC容器
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public virtual void Register(string key, object value)
        {
            DebugCheck.NotNull(value);
            // DebugCheck.NotNull(key);

            _initializers.GetOrAdd(ValueTuple.Create(value.GetType(), key), value);
            var interFaces = value.GetType().GetTypeInfo().GetInterfaces();
            foreach (var interFace in interFaces)
            {
                _initializers.GetOrAdd(ValueTuple.Create(interFace, key), value);
            }
        }

        public virtual void Register(string key, object value, Type type)
        {
            DebugCheck.NotNull(value);
            _initializers.GetOrAdd(ValueTuple.Create(type, key), value);
        }

        /// <summary>
        /// 返回当前IOC容器
        /// </summary>
        public static ServiceResolver Current
        {
            get { return _defaultInstance; }
        }

        /// <summary>
        /// 通过KEY和TYPE获取实例对象
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="key">键</param>
        /// <returns>返回实例对象</returns>
        public virtual object GetService(Type type, object key)
        {
            object result;
            _initializers.TryGetValue(ValueTuple.Create(type, key == null ? null : key.ToString()), out result);
            return result;
        }

        /// <summary>
        /// 通过KEY和TYPE获取实例对象集合
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="key">键</param>
        /// <returns>返回实例对象</returns>
        public IEnumerable<object> GetServices(Type type, object key)
        {
            return this.GetServiceAsServices(type, key);
        }
        #endregion
    }
}
