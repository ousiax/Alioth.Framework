/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;

namespace Alioth.Framework {
    /// <summary>
    /// Defines a mechanism for retrieving a service object.
    /// </summary>
    public interface IAliothServiceProvider : IServiceProvider {
        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An <c>System.Type</c> that specifies the type of service object to get.</param>
        /// <param name="name">An <c>System.String</c> that specifies the name of service object to get.</param>
        /// <param name="version">An <c>System.String</c> that specifies the version of service object to get.</param>
        /// <returns>A service object of type serviceType.-or- null if there is no service object of type serviceType.</returns>
        object GetService(Type serviceType, string name, string version);
    }
}
