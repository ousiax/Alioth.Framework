/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System.Collections.Generic;

namespace Alioth.Framework.Config
{
    /// <summary>
    /// Represents a service container that contains serivce metadata configuration.
    /// </summary>
    public class ServiceContainer
    {
        private List<Service> services;

        /// <summary>
        /// Gets or sets the service metadata list.
        /// </summary>
        public List<Service> Services
        {
            get { return services; }
            set { services = value; }
        }

        /// <summary>
        /// Initializes a new instance of the class <c>Alioth.Framework.Config.ServiceContainer</c>.
        /// </summary>
        public ServiceContainer()
        {
            this.services = new List<Service>();
        }
    }
}
