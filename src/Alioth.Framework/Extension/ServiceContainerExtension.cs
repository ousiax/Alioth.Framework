/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;

namespace Alioth.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceContainerExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static IAliothServiceContainer Apply(this IAliothServiceContainer container, Type objectType)
        {
            return container.Apply(objectType, null, null, null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="objectType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IAliothServiceContainer Apply(this IAliothServiceContainer container, Type objectType, string name)
        {
            return container.Apply(objectType, null, null, name, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="objectType"></param>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static IAliothServiceContainer Apply(this IAliothServiceContainer container, Type objectType, string name, string version)
        {
            return container.Apply(objectType, null, null, name, version);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="container"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static IAliothServiceContainer Apply<TService, TImplementation>(this IAliothServiceContainer container, TImplementation instance) where TImplementation : TService
        {
            return container.Apply<TService, TImplementation>(instance, null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="container"></param>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IAliothServiceContainer Apply<TService, TImplementation>(this IAliothServiceContainer container, TImplementation instance, string name) where TImplementation : TService
        {
            return container.Apply<TService, TImplementation>(instance, name, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="container"></param>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static IAliothServiceContainer Apply<TService, TImplementation>(this IAliothServiceContainer container, TImplementation instance, string name, string version) where TImplementation : TService
        {
            return container.Apply<TService, TImplementation>(instance, name, version);
        }
    }
}
