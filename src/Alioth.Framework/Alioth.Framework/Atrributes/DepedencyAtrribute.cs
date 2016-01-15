/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;

namespace Alioth.Framework {
    [AttributeUsage(AttributeTargets.Property
        | AttributeTargets.Constructor
        | AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class DepedencyAtrribute : Attribute {
        private Type serviceType;
        private String serviceName;
        private String serviceVersion;

        public Type ServiceType {
            get { return serviceType; }
            set { serviceType = value; }
        }

        public String ServiceName {
            get { return serviceName; }
            set { serviceName = value; }
        }

        public String ServiceVersion {
            get { return serviceVersion; }
            set { serviceVersion = value; }
        }

        public DepedencyAtrribute() {
        }

        public DepedencyAtrribute(Type serviceType) {
            #region precondition
            if (serviceType == null) {
                throw new ArgumentNullException("serviceType");
            }
            #endregion
            this.serviceType = serviceType;
        }
    }
}
