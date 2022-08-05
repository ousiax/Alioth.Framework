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
    /// Specifies the type of the service object class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ServiceTypeAttribute : Attribute
    {
        /// <summary>
        /// Gets the service type.
        /// <see cref="ServiceKey.Type"/>.
        /// </summary>
        public Type ServiceType { get; }

        /// <summary>
        /// Gets the service reference type.
        /// <see cref="ReferenceType"/>.
        /// </summary>
        public ReferenceType ReferenceType { get; }

        /// <summary>
        /// Initializes a new instance of the class <c>Alioth.Framework.ServiceTypeAtrribute</c>.
        /// </summary>
        /// <param name="serviceType">the service type.</param>
        /// <param name="referenceType">the service reference type.</param>
        public ServiceTypeAttribute(Type serviceType, ReferenceType referenceType = ReferenceType.Strong)
        {
            ServiceType = serviceType;
            ReferenceType = referenceType;
        }
    }
}
