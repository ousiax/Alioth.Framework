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
    /// Represents a service object metadata configuration.
    /// </summary>
    public class Service
    {
        /// <summary>
        /// Gets or set the type of the serivce object class.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the name of the serivce type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the version of the serivce type.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the description information of the serivce type.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the parameters dictionary of the serivce type.
        /// </summary>
        public IDictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the properties dictionary of the serivce type.
        /// </summary>
        public IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

        /// <summary>
        ///  Initializes a new instance of the class <c>Alioth.Framework.Config.Service</c>.
        /// </summary>
        public Service()
        {
        }
    }
}
