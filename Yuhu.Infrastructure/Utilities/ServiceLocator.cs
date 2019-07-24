﻿using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yuhu.Infrastructure.Utilities
{
    /// <summary>
    /// 服务定位帮助类
    /// </summary>
    public class ServiceLocator
    {
        /// <summary>
        /// autofac 容器
        /// </summary>
        public static IContainer Current { get; set; }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>()
        {
            return Current.Resolve<T>();
        }

        /// <summary>
        /// 是否注册服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsRegistered<T>()
        {
            return Current.IsRegistered<T>();
        }

        /// <summary>
        /// 是否注册服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">注册服务时的关键字</param>
        /// <returns></returns>
        public static bool IsRegistered<T>(string key)
        {
            return Current.IsRegisteredWithKey<T>(key);
        }

        /// <summary>
        /// 是否注册服务
        /// </summary>
        /// <param name="type">服务类型</param>
        /// <returns></returns>
        public static bool IsRegistered(Type type)
        {
            return Current.IsRegistered(type);
        }

        /// <summary>
        /// 是否已经注册类型为<paramref name="type"/>，关键字为<paramref name="key"/>的服务
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsRegisteredWithKey(string key, Type type)
        {
            return Current.IsRegisteredWithKey(key, type);
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetService<T>(string key)
        {

            return Current.ResolveKeyed<T>(key);
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetService(Type type)
        {
            return Current.Resolve(type);
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetService(string key, Type type)
        {
            return Current.ResolveKeyed(key, type);
        }
    }
}
