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
    public class ServiceTypeAtrribute : Attribute
    {
        private Type serviceType;
        private ReferenceType referenceType;

        /// <summary>
        /// Gets the service type.
        /// <see cref="Alioth.Framework.ServiceKey.Type"/>.
        /// </summary>
        public Type ServiceType { get { return this.serviceType; } }

        /// <summary>
        /// Gets the service reference type.
        /// <see cref="Alioth.Framework.ReferenceType"/>.
        /// </summary>
        public ReferenceType ReferenceType { get { return this.referenceType; } }

        /// <summary>
        /// Initializes a new instance of the class <c>Alioth.Framework.ServiceTypeAtrribute</c>.
        /// </summary>
        /// <param name="serviceType">the service type.</param>
        /// <param name="referenceType">the service reference type.</param>
        public ServiceTypeAtrribute(Type serviceType, ReferenceType referenceType = ReferenceType.Strong)
        {
            this.serviceType = serviceType;
            this.referenceType = referenceType;
        }
    }
}
