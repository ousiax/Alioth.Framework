/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;

namespace Alioth.Framework {
    public struct ServiceKey {
        private Type type;
        private String name;
        private String version;
        private int hashCode;

        public Type Type {
            get { return type; }
        }

        public String Name {
            get { return name; }
        }

        public String Version {
            get { return version; }
        }

        public ServiceKey(Type type, String name, String version) {
            #region precondition
            if (type == null) {
                throw new ArgumentNullException("type");
            }
            #endregion
            this.type = type;
            this.name = name ?? String.Empty;
            this.version = version ?? String.Empty;

            this.hashCode = 0;
            int hashCode = this.type.GetHashCode();
            if (name != null) {
                hashCode = 29 * hashCode | name.GetHashCode();
            }
            if (version != null) {
                hashCode = 29 * hashCode | version.GetHashCode();
            }
        }

        public static ServiceKey Create(Type type) {
            return new ServiceKey(type, null, null);
        }

        public static ServiceKey Create(Type type, String name) {
            return new ServiceKey(type, name, null);
        }

        public static ServiceKey Create(Type type, String name, String version) {
            return new ServiceKey(type, name, version);
        }

        public static Boolean operator ==(ServiceKey key1, ServiceKey key2) {
            return key1.Equals(key2);
        }

        public static Boolean operator !=(ServiceKey key1, ServiceKey key2) {
            return !key1.Equals(key2);
        }

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

        public override int GetHashCode() {
            return hashCode;
        }

        public override string ToString() {
            return String.Format("Type:{0}, Name:{1}, Version:{2}", this.type.Name, this.name, this.version);
        }
    }
}
