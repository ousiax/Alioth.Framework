/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;

namespace Alioth.Framework {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ServiceTypeAtrribute : Attribute {
        private Type serviceType;
        private ReferenceType referenceType;

        public Type ServiceType { get { return this.serviceType; } }

        public ReferenceType ReferenceType { get { return this.referenceType; } }

        public ServiceTypeAtrribute(Type serviceType, ReferenceType referenceType = ReferenceType.Strong) {
            this.serviceType = serviceType;
            this.referenceType = referenceType;
        }
    }
}
