/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;
using System.Collections.Generic;

namespace Alioth.Framework
{
    /// <summary>
    /// Defines a mechanism for building service object.
    /// </summary>
    internal interface IObjectBuilder : IAliothServiceContainerConnector
    {
        /// <summary>
        /// Gets or sets the service object class type.
        /// </summary>
        Type ObjectType { get; set; }

        /// <summary>
        /// Gets the parameters dictioinary of the service object.
        /// </summary>
        IDictionary<String, String> Parameters { get; }

        /// <summary>
        /// Gets the properties dictioinary of the service object.
        /// </summary>
        IDictionary<String, String> Properties { get; }

        /// <summary>
        /// Builds a new instance of the service object.
        /// </summary>
        /// <returns>An object instance of the service object class.</returns>
        Object Build();
    }
}
