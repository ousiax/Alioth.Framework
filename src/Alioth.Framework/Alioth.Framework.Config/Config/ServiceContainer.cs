/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;
using System.Collections.Generic;

namespace Alioth.Framework.Config {
    [Serializable]
    public class ServiceContainer {
        private List<Service> services;

        public List<Service> Services {
            get { return services; }
            set { services = value; }
        }

        public ServiceContainer() {
            this.services = new List<Service>();
        }
    }
}
