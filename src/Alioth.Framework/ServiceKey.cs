/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;
using System.Globalization;

namespace Alioth.Framework
{
    /// <summary>
    /// Represents a service key to identify a service object type.
    /// </summary>
    internal struct ServiceKey
    {
        private readonly int _hashCode;

        /// <summary>
        /// Gets the service type of the <c>Alioth.Framework.ServiceKey</c>.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets the service name of the <c>Alioth.Framework.ServiceKey</c>.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the service version of the <c>Alioth.Framework.ServiceKey</c>.
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// Initializes a new instance of the struct <c>Alioth.Framework.ServiceKey</c>.
        /// </summary>
        /// <param name="type">A <c>System.Type</c> that specifies the type of service object to get.</param>
        /// <param name="name">An <c>System.String</c> that specifies the name of service object to get.</param>
        /// <param name="version">An <c>System.String</c> that specifies the version of service object to get.</param>
        public ServiceKey(Type type, string name, string version)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Name = (name ?? string.Empty).ToLower(CultureInfo.InvariantCulture);
            Version = (version ?? string.Empty).ToLower(CultureInfo.InvariantCulture);

            _hashCode = Type.FullName.GetHashCode();
            //if (name != null)
            //{
            //    _hashCode = (29 * _hashCode) | name.GetHashCode();
            //}
            //if (version != null)
            //{
            //    _hashCode = (29 * _hashCode) | version.GetHashCode();
            //}
        }

        /// <summary>
        /// Creates a new instance of the struct <c>Alioth.Framework.ServiceKey</c>.
        /// </summary>
        /// <param name="type">A <c>System.Type</c> that specifies the type of service object to get.</param>
        /// <returns>A new instance of the struct <c>Alioth.Framework.ServiceKey</c>.</returns>
        public static ServiceKey Create(Type type)
        {
            return new ServiceKey(type, null, null);
        }

        /// <summary>
        /// Creates a new instance of the struct <c>Alioth.Framework.ServiceKey</c>.
        /// </summary>
        /// <param name="type">A <c>System.Type</c> that specifies the type of service object to get.</param>
        /// <param name="name">An <c>System.String</c> that specifies the name of service object to get.</param>
        /// <returns>A new instance of the struct <c>Alioth.Framework.ServiceKey</c>.</returns>
        public static ServiceKey Create(Type type, string name)
        {
            return new ServiceKey(type, name, null);
        }

        /// <summary>
        /// Creates a new instance of the struct <c>Alioth.Framework.ServiceKey</c>.
        /// </summary>
        /// <param name="type">A <c>System.Type</c> that specifies the type of service object to get.</param>
        /// <param name="name">An <c>System.String</c> that specifies the name of the type of service object to get.</param>
        /// <param name="version">An <c>System.String</c> that specifies the version of the type of service object type to get.</param>
        /// <returns>A new instance of the struct <c>Alioth.Framework.ServiceKey</c>.</returns>
        public static ServiceKey Create(Type type, string name, string version)
        {
            return new ServiceKey(type, name, version);
        }

        /// <summary>
        /// Determines whether two specified <c>Alioth.Framework.ServiceKey</c> objects represent the same service key.
        /// </summary>
        /// <param name="key1">The first object to compare.</param>
        /// <param name="key2">The second object to compare.</param>
        /// <returns>true if both <c>Alioth.Framework.ServiceKey</c> objects have the same <c>Alioth.Framework.ServiceKey</c> value; otherwise, false.</returns>
        public static bool operator ==(ServiceKey key1, ServiceKey key2)
        {
            return key1.Equals(key2);
        }

        /// <summary>
        /// Determines whether two specified <c>Alioth.Framework.ServiceKey</c> objects refer to different service key.
        /// </summary>
        /// <param name="key1">The first object to compare.</param>
        /// <param name="key2">The second object to compare.</param>
        /// <returns>true if both <c>Alioth.Framework.ServiceKey</c> objects refer to different service key; otherwise, false.</returns>
        public static bool operator !=(ServiceKey key1, ServiceKey key2)
        {
            return !key1.Equals(key2);
        }

        /// <summary>
        /// Determines whether the current <c>Alioth.Framework.ServiceKey</c> object represents the same service key as a specified <c>Alioth.Framework.ServiceKey</c> object.
        /// </summary>
        /// <param name="obj">An object to compare to the current <c>Alioth.Framework.ServiceKey</c> object.</param>
        /// <returns>true if both <c>Alioth.Framework.ServiceKey</c> objects have the same <c>Alioth.Framework.ServiceKey</c> value; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }
            if (obj.GetType() == GetType())
            {
                var key = (ServiceKey)obj;
                return Type.Equals(key.Type)
                    && Name.Equals(key.Name, StringComparison.OrdinalIgnoreCase)
                    && Version.Equals(key.Version, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        /// <summary>
        /// Returns the hash code for the current <c>Alioth.Framework.ServiceKey</c> object.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return _hashCode;
        }

        /// <summary>
        /// Converts the value of the current <c>Alioth.Framework.ServiceKey</c> object to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of a <c>Alioth.Framework.ServiceKey</c> object.</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Type:{0}, Name:{1}, Version:{2}", Type.Name, Name, Version);
        }
    }
}
