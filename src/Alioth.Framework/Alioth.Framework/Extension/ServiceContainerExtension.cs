/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;

namespace Alioth.Framework {

    public static class ServiceContainerExtension {
        public static IAliothServiceContainer Apply(this IAliothServiceContainer container, Type objectType) {
            return container.Apply(objectType, null, null);
        }

        public static IAliothServiceContainer Apply(this IAliothServiceContainer container, Type objectType, string name) {
            return container.Apply(objectType, name, null);
        }

        public static IAliothServiceContainer Apply<T, O>(this IAliothServiceContainer container, O instance) where O : T {
            return container.Apply<T, O>(instance, null, null);
        }

        public static IAliothServiceContainer Apply<T, O>(this IAliothServiceContainer container, O instance, String name) where O : T {
            return container.Apply<T, O>(instance, name, null);
        }

        public static IAliothServiceContainer Apply<T, O>(this IAliothServiceContainer container, O instance, String name, String version) where O : T {
            return container.Apply<T, O>(instance, name, version);
        }
    }
}
