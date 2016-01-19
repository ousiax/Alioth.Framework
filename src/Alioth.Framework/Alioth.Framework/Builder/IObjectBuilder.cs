/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;
using System.Collections.Generic;

namespace Alioth.Framework {
    internal interface IObjectBuilder : IAliothServiceContainerConnector {
        Type ObjectType { get; set; }

        IDictionary<String, String> Parameters { get; }

        IDictionary<String, String> Properties { get; }

        Object Build();
    }
}
