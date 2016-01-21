/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;

namespace Alioth.Framework {
    /// <summary>
    /// Represents a service key to identify a service object type.
    /// </summary>
    internal struct ServiceKey {
        private Type type;
        private String name;
        private String version;
        private int hashCode;

        /// <summary>
        /// Gets the service type of the <c>Alioth.Framework.ServiceKey</c>.
        /// </summary>
        public Type Type {
            get { return type; }
        }

        /// <summary>
        /// Gets the service name of the <c>Alioth.Framework.ServiceKey</c>.
        /// </summary>
        public String Name {
            get { return name; }
        }

        /// <summary>
        /// Gets the service version of the <c>Alioth.Framework.ServiceKey</c>.
        /// </summary>
        public String Version {
            get { return version; }
        }

        /// <summary>
        /// Initializes a new instance of the struct <c>Alioth.Framework.ServiceKey</c>.
        /// </summary>
        /// <param name="type">A <c>System.Type</c> that specifies the type of service object to get.</param>
        /// <param name="name">An <c>System.String</c> that specifies the name of service object to get.</param>
        /// <param name="version">An <c>System.String</c> that specifies the version of service object to get.</param>
        public ServiceKey(Type type, String name, String version) {
            #region precondition
            if (type == null) {
                throw new ArgumentNullException("type");
            }
            #endregion
            this.type = type;
            this.name = (name ?? String.Empty).ToLower();
            this.version = (version ?? String.Empty).ToLower();

            this.hashCode = 0;
            int hashCode = this.type.GetHashCode();
            if (name != null) {
                hashCode = 29 * hashCode | name.GetHashCode();
            }
            if (version != null) {
                hashCode = 29 * hashCode | version.GetHashCode();
            }
        }

        /// <summary>
        /// Creates a new instance of the struct <c>Alioth.Framework.ServiceKey</c>.
        /// </summary>
        /// <param name="type">A <c>System.Type</c> that specifies the type of service object to get.</param>
        /// <returns>A new instance of the struct <c>Alioth.Framework.ServiceKey</c>.</returns>
        public static ServiceKey Create(Type type) {
            return new ServiceKey(type, null, null);
        }

        /// <summary>
        /// Creates a new instance of the struct <c>Alioth.Framework.ServiceKey</c>.
        /// </summary>
        /// <param name="type">A <c>System.Type</c> that specifies the type of service object to get.</param>
        /// <param name="name">An <c>System.String</c> that specifies the name of service object to get.</param>
        /// <returns>A new instance of the struct <c>Alioth.Framework.ServiceKey</c>.</returns>
        public static ServiceKey Create(Type type, String name) {
            return new ServiceKey(type, name, null);
        }

        /// <summary>
        /// Creates a new instance of the struct <c>Alioth.Framework.ServiceKey</c>.
        /// </summary>
        /// <param name="type">A <c>System.Type</c> that specifies the type of service object to get.</param>
        /// <param name="name">An <c>System.String</c> that specifies the name of the type of service object to get.</param>
        /// <param name="version">An <c>System.String</c> that specifies the version of the type of service object type to get.</param>
        /// <returns>A new instance of the struct <c>Alioth.Framework.ServiceKey</c>.</returns>
        public static ServiceKey Create(Type type, String name, String version) {
            return new ServiceKey(type, name, version);
        }

        /// <summary>
        /// Determines whether two specified <c>Alioth.Framework.ServiceKey</c> objects represent the same service key.
        /// </summary>
        /// <param name="key1">The first object to compare.</param>
        /// <param name="key2">The second object to compare.</param>
        /// <returns>true if both <c>Alioth.Framework.ServiceKey</c> objects have the same <c>Alioth.Framework.ServiceKey</c> value; otherwise, false.</returns>
        public static Boolean operator ==(ServiceKey key1, ServiceKey key2) {
            return key1.Equals(key2);
        }

        /// <summary>
        /// Determines whether two specified <c>Alioth.Framework.ServiceKey</c> objects refer to different service key.
        /// </summary>
        /// <param name="key1">The first object to compare.</param>
        /// <param name="key2">The second object to compare.</param>
        /// <returns>true if both <c>Alioth.Framework.ServiceKey</c> objects refer to different service key; otherwise, false.</returns>
        public static Boolean operator !=(ServiceKey key1, ServiceKey key2) {
            return !key1.Equals(key2);
        }

        /// <summary>
        /// Determines whether the current <c>Alioth.Framework.ServiceKey</c> object represents the same service key as a specified <c>Alioth.Framework.ServiceKey</c> object.
        /// </summary>
        /// <param name="obj">An object to compare to the current <c>Alioth.Framework.ServiceKey</c> object.</param>
        /// <returns>true if both <c>Alioth.Framework.ServiceKey</c> objects have the same <c>Alioth.Framework.ServiceKey</c> value; otherwise, false.</returns>
        public override Boolean Equals(object obj) {
            if (Object.ReferenceEquals(obj, null)) {
                return false;
            }
            if (obj.GetType() == this.GetType()) {
                ServiceKey key = (ServiceKey)obj;
                return this.type.Equals(key.type)
                    && this.name.Equals(key.name, StringComparison.InvariantCultureIgnoreCase)
                    && this.version.Equals(key.version, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }

        /// Returns the hash code for the current <c>Alioth.Framework.ServiceKey</c> object.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode() {
            return hashCode;
        }

        /// <summary>
        /// Converts the value of the current <c>Alioth.Framework.ServiceKey</c> object to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of a <c>Alioth.Framework.ServiceKey</c> object.</returns>
        public override string ToString() {
            return String.Format("Type:{0}, Name:{1}, Version:{2}", this.type.Name, this.name, this.version);
        }
    }
}
