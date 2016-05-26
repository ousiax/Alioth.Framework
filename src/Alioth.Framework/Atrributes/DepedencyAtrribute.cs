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
    public class DepedencyAtrribute : Attribute
    {
        private Type serviceType;
        private String serviceName;
        private String serviceVersion;

        /// <summary>
        /// Gets or sets the service type.
        /// <see seealso="Alioth.Framework.ServiceKey.Type"/>
        /// </summary>
        public Type ServiceType
        {
            get { return serviceType; }
            set { serviceType = value; }
        }

        /// <summary>
        /// Gets or sets the service name.
        /// <seealso cref="Alioth.Framework.ServiceKey.Name"/>
        /// </summary>
        public String ServiceName
        {
            get { return serviceName; }
            set { serviceName = value; }
        }

        /// <summary>
        /// Gets or sets the service version.
        /// <seealso cref="Alioth.Framework.ServiceKey.Version"/>
        /// </summary>
        public String ServiceVersion
        {
            get { return serviceVersion; }
            set { serviceVersion = value; }
        }

        /// <summary>
        /// Initialize a new instance of class <c>Alioth.Framework.DepedencyAtrribute</c>.
        /// </summary>
        public DepedencyAtrribute()
        {
        }

        /// <summary>
        /// Initialize a new instance of class <c>Alioth.Framework.DepedencyAtrribute</c> with a specified <paramref name="serviceType"/>.
        /// </summary>
        /// <param name="serviceType"><see cref="Alioth.Framework.ServiceKey.Type"/></param>
        public DepedencyAtrribute(Type serviceType)
        {
            #region precondition
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            #endregion
            this.serviceType = serviceType;
        }
    }
}
