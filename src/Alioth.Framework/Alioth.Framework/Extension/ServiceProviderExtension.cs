/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;

namespace Alioth.Framework {

    public static class ServiceProviderExtension {

        public static T GetService<T>(this IAliothServiceProvider provider, Type serviceType) {
            return (T)provider.GetService<T>(serviceType, null, null);
        }

        public static T GetService<T>(this IAliothServiceProvider provider, Type serviceType, String name) {
            return (T)provider.GetService<T>(serviceType, name, null);
        }

        public static T GetService<T>(this IAliothServiceProvider provider, Type serviceType, String name, String version) {
            return (T)provider.GetService(serviceType, name, version);
        }
    }
}
