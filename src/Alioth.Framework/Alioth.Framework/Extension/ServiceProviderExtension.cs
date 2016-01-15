/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;

namespace Alioth.Framework {

    public static class ServiceProviderExtension {

        public static T GetService<T>(this IAliothServiceProvider provider) {
            return (T)provider.GetService(typeof(T), null, null);
        }

        public static T GetService<T>(this IAliothServiceProvider provider, String name) {
            return (T)provider.GetService(typeof(T), name, null);
        }

        public static T GetService<T>(this IAliothServiceProvider provider, String name, String version) {
            return (T)provider.GetService(typeof(T), name, version);
        }
    }
}
