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
    public class Service {
        private String type;
        private String name;
        private String version;
        private String description;
        private IDictionary<String, String> parameters;
        private IDictionary<String, String> properties;

        public String Type {
            get { return type; }
            set { type = value; }
        }

        public String Name {
            get { return name; }
            set { name = value; }
        }

        public String Version {
            get { return version; }
            set { version = value; }
        }

        public String Description {
            get { return description; }
            set { description = value; }
        }

        public IDictionary<String, String> Parameters {
            get { return parameters; }
            set { parameters = value; }
        }

        public IDictionary<String, String> Properties {
            get { return properties; }
            set { properties = value; }
        }

        public Service() {
            this.parameters = new Dictionary<String, String>();
            this.properties = new Dictionary<String, String>();
        }
    }
}
