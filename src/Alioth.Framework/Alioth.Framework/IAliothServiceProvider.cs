/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;

namespace Alioth.Framework {

    public interface IAliothServiceProvider : IServiceProvider {
        object GetService(Type serviceType, string name, string version);
    }
}
