/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;

namespace Alioth.Framework
{
    /// <summary>
    /// Specifies the dependent properties, constructors, and parameters that planted in a service object constructor.
    /// </summary>
    /// <remarks>A property that annotated with <c>Alioth.Framework.DepedencyAtrribute</c> will be injected with property injection of Ioc.
    /// <para>A constructor that annotated with <c>Alioth.Framework.DepedencyAtrribute</c> to determine a dependent constructor to create object instance when multiple construtors are defined.</para>
    /// <para>A constructor parameter that annotated with <c>Alioth.Framework.DepedencyAtrribute</c> to provide a paramter from the IoC container.</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property
        | AttributeTargets.Constructor
        | AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public sealed class DepedencyAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the service type.
        /// <see seealso="Alioth.Framework.ServiceKey.Type"/>
        /// </summary>
        public Type ServiceType { get; set; }

        /// <summary>
        /// Gets or sets the service name.
        /// <seealso cref="ServiceKey.Name"/>
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the service version.
        /// <seealso cref="ServiceKey.Version"/>
        /// </summary>
        public string ServiceVersion { get; set; }

        /// <summary>
        /// Initialize a new instance of class <c>Alioth.Framework.DepedencyAtrribute</c>.
        /// </summary>
        public DepedencyAttribute()
        {
        }

        /// <summary>
        /// Initialize a new instance of class <c>Alioth.Framework.DepedencyAtrribute</c> with a specified <paramref name="serviceType"/>.
        /// </summary>
        /// <param name="serviceType"><see cref="ServiceKey.Type"/></param>
        public DepedencyAttribute(Type serviceType)
        {
            ServiceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
        }
    }
}
