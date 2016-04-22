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
            var s = provider.GetService(typeof(T), null, null);
            return s == null ? default(T) : (T)s;
        }

        public static T GetService<T>(this IAliothServiceProvider provider, String name) {
            var s = (T)provider.GetService(typeof(T), name, null);
            return s == null ? default(T) : (T)s;
        }

        public static T GetService<T>(this IAliothServiceProvider provider, String name, String version) {
            var s = (T)provider.GetService(typeof(T), name, version);
            return s == null ? default(T) : (T)s;
        }
    }
}
