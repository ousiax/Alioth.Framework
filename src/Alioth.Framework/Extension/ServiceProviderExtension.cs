/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

namespace Alioth.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceProviderExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static T GetService<T>(this IAliothServiceProvider provider)
        {
            var s = provider.GetService(typeof(T), null, null);
            return s == null ? default : (T)s;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetService<T>(this IAliothServiceProvider provider, string name)
        {
            var s = (T)provider.GetService(typeof(T), name, null);
            return s == null ? default : s;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static T GetService<T>(this IAliothServiceProvider provider, string name, string version)
        {
            var s = (T)provider.GetService(typeof(T), name, version);
            return s == null ? default : s;
        }
    }
}
