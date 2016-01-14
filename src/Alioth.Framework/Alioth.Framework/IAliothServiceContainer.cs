/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;

namespace Alioth.Framework {

    public interface IAliothServiceContainer : IAliothServiceProvider {
        String Description { get; set; }

        IAliothServiceContainer Parent { get; }

        IAliothServiceContainer Apply(Type objectType, String name, String version);

        IAliothServiceContainer Apply<T, O>(O instance, String name, String version) where O : T;

        IAliothServiceContainer CreateChild(String name = null);
    }
}
