/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;
using System.Collections.Generic;

namespace Alioth.Framework.Config {
    /// <summary>
    /// Represents a service object metadata configuration.
    /// </summary>
    [Serializable]
    public class Service {
        private String type;
        private String name;
        private String version;
        private String description;
        private IDictionary<String, String> parameters;
        private IDictionary<String, String> properties;

        /// <summary>
        /// Gets or set the type of the serivce object class.
        /// </summary>
        public String Type {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Gets or sets the name of the serivce type.
        /// </summary>
        public String Name {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets or sets the version of the serivce type.
        /// </summary>
        public String Version {
            get { return version; }
            set { version = value; }
        }

        /// <summary>
        /// Gets or sets the description information of the serivce type.
        /// </summary>
        public String Description {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Gets or sets the parameters dictionary of the serivce type.
        /// </summary>
        public IDictionary<String, String> Parameters {
            get { return parameters; }
            set { parameters = value; }
        }

        /// <summary>
        /// Gets or sets the properties dictionary of the serivce type.
        /// </summary>
        public IDictionary<String, String> Properties {
            get { return properties; }
            set { properties = value; }
        }

        /// <summary>
        ///  Initializes a new instance of the class <c>Alioth.Framework.Config.Service</c>.
        /// </summary>
        public Service() {
            this.parameters = new Dictionary<String, String>();
            this.properties = new Dictionary<String, String>();
        }
    }
}
