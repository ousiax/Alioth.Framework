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
    /// Represents a IoC container.
    /// </summary>
    public interface IAliothServiceContainer : IAliothServiceProvider
    {
        /// <summary>
        /// Gets or sets the description of the IoC container.
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// Gets the parent IoC cotnainer.
        /// </summary>
        IAliothServiceContainer Parent { get; }

        /// <summary>
        /// Applys a service object type that used to create service objects.
        /// </summary>
        /// <param name="objectType">A <c>System.Type</c> that specifies the service object type to create service object.</param>
        /// <param name="parameters">A <c>IDictionary&lt;String, String^gt;</c> that specifies a dictionary of the dependency constructor of the service object type.</param>
        /// <param name="properties">A <c>IDictionary&lt;String, String^gt;</c> specifies a dictionary to set properties of the service object.</param>
        /// <param name="name">An <c>System.String</c> that specifies the name of service object to get.</param>
        /// <param name="version">An <c>System.String</c> that specifies the version of service object to get.</param>
        /// <returns>An IoC container that implements <c>Alioth.Framework.IAliothServiceContainer</c>.</returns>
        IAliothServiceContainer Apply(Type objectType, IDictionary<String, String> parameters, IDictionary<String, String> properties, String name, String version);

        /// <summary>
        /// Applys a singleton service object.
        /// </summary>
        /// <typeparam name="T">The service type of the service object.</typeparam>
        /// <typeparam name="O">The type of the service object.</typeparam>
        /// <param name="instance">The service object.</param>
        /// <param name="name">An <c>System.String</c> that specifies the name of service object to get.</param>
        /// <param name="version">An <c>System.String</c> that specifies the version of service object to get.</param>
        /// <returns>An IoC container that implements <c>Alioth.Framework.IAliothServiceContainer</c>.</returns>
        IAliothServiceContainer Apply<T, O>(O instance, String name, String version) where O : T;

        /// <summary>
        /// Creates a child IoC container.
        /// </summary>
        /// <param name="description">A <c>System.String</c> that represents the description of the child IoC container.</param>
        /// <returns>An IoC container that implements <c>Alioth.Framework.IAliothServiceContainer</c>.</returns>
        IAliothServiceContainer CreateChild(String description = null);
    }
}
