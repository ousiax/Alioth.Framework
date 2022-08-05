/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Alioth.Framework
{
    /// <summary>
    /// Represents a service object builder to build service object.
    /// </summary>
    [DebuggerDisplay("Type={objectType}")]
    internal class ObjectBuilder : IObjectBuilder, IAliothServiceProvider
    {
        private IAliothServiceContainer _container;
        private Type _objectType;

        /// <summary>
        /// Gets or sets the service object class type.
        /// </summary>
        public Type ObjectType
        {
            get => _objectType;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (!value.GetTypeInfo().IsClass)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "The specified object type should be a concrete class.");
                }
                _objectType = value;
            }
        }

        /// <summary>
        /// Gets the parameters dictioinary of the service object.
        /// </summary>
        public IDictionary<string, string> Parameters { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets the properties dictioinary of the service object.
        /// </summary>
        public IDictionary<string, string> Properties { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the class <c>Alioth.Framework.ObjectBuilder</c>.
        /// </summary>
        public ObjectBuilder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the class <c>Alioth.Framework.ObjectBuilder</c>.
        /// </summary>
        /// <param name="objectType">The service object class type.</param>
        public ObjectBuilder(Type objectType) : this()
        {

            if (objectType == null)
            {
                throw new ArgumentNullException(nameof(objectType));
            }

            if (!objectType.GetTypeInfo().IsClass)
            {
                throw new ArgumentOutOfRangeException(nameof(objectType), "The specified object type should be a concrete class.");
            }

            _objectType = objectType;
        }

        /// <summary>
        /// Builds a new instance of the service object.
        /// </summary>
        /// <returns>An object instance of the service object class.</returns>
        public virtual object Build()
        {

            ConstructorInfo[] ctors = _objectType.GetTypeInfo().DeclaredConstructors.ToArray();
            if (ctors.Length > 1)
            {
                var ctors2 = ctors.Where(o => o.GetCustomAttributes(false).Any(p => p.GetType() == typeof(DepedencyAttribute))).ToArray();
                if (ctors2.Length > 1)
                {
                    throw new InvalidOperationException($"Too many constructors annotated with Alioth.Framework.DepedencyAtrribute. Type: {ObjectType.AssemblyQualifiedName}.");
                }
                else if (ctors2.Length == 0)
                {
                    throw new InvalidOperationException($"Too many constructors but no one is annotated with Alioth.Framework.DepedencyAtrribute. Type: {ObjectType.AssemblyQualifiedName}");
                }
                return Create(ctors2[0]);
            }
            else
            {
                return Create(ctors[0]);
            }
        }

        public void Connect(IAliothServiceContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public object GetService(Type serviceType)
        {
            return _container.GetService(serviceType);
        }

        public object GetService(Type serviceType, string name, string version)
        {
            return _container.GetService(serviceType, name, version);
        }

        private object Create(ConstructorInfo ctor)
        {
            ParameterInfo[] ps = ctor.GetParameters();
            var args = new object[ps.Length];
            for (int i = 0; i < ps.Length; i++)
            {
                ParameterInfo pi = ps[i];
                object value = GetParameterValue(pi);
                args[i] = value;
            }
            var instance = ctor.Invoke(args);
            ConnectContainer(instance);
            InjectDepedencyProperties(instance);
            InjectProperties(instance);
            return instance;
        }

        private object GetParameterValue(ParameterInfo pi)
        {
            var depAttrs = (DepedencyAttribute[])pi.GetCustomAttributes(typeof(DepedencyAttribute), false);
            object value;
            if (depAttrs.Length == 0)
            {
                Type type = pi.ParameterType;
                var name = pi.Name;
                if (Parameters.TryGetValue(name, out var s))
                {
                    value = ParseValue(type, s);
                }
                else if ((pi.Attributes & ParameterAttributes.HasDefault) == ParameterAttributes.HasDefault)
                {
                    value = pi.DefaultValue;
                }
                else
                {
                    throw new ArgumentException($"Parameter \"{pi.Name}\" must be provided. Type: {ObjectType.AssemblyQualifiedName}", pi.Name);
                }
            }
            else
            {
                DepedencyAttribute depAttr = depAttrs[0];
                value = GetService(depAttr.ServiceType, depAttr.ServiceName, depAttr.ServiceVersion);
            }

            return value;
        }

        private void ConnectContainer(object instance)
        {
            if (instance is IAliothServiceContainerConnector conn)
            {
                conn.Connect(_container);
            }
        }

        private void InjectDepedencyProperties(object instance)
        {
            PropertyInfo[] properties = _objectType.GetTypeInfo()
                .DeclaredProperties
                .Where(p => p.GetCustomAttributes(false).Any(s => s.GetType() == typeof(DepedencyAttribute)))
                .ToArray();
            foreach (PropertyInfo p in properties)
            {
                DepedencyAttribute attr = p.GetCustomAttributes<DepedencyAttribute>().First();
                var v = GetService(attr.ServiceType, attr.ServiceName, attr.ServiceVersion);
                if (v == null)
                {
                    throw new KeyNotFoundException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "The sepecified depedency service with a key '{0}' could not be found. Type: {1}",
                            ServiceKey.Create(attr.ServiceType, attr.ServiceName, attr.ServiceVersion), ObjectType.AssemblyQualifiedName));
                }
                p.SetValue(instance, v, null);
            }
        }

        private void InjectProperties(object instance)
        {
            if (Properties.Count > 0)
            {
                TypeInfo t = instance.GetType().GetTypeInfo();
                foreach (var item in Properties)
                {
                    PropertyInfo pi = t.GetDeclaredProperty(item.Key);
                    if (pi == null)
                    {
                        throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, "Invalid [Properites] Configuration: could not found a property with the specified name '{0}'. Type: {1}", item.Key, ObjectType.AssemblyQualifiedName));
                    }
                    pi.SetValue(instance, ParseValue(pi.PropertyType, item.Value), null);
                }
            }
        }

        private static object ParseValue(Type type, string rawString)
        {
            object value = null;
            if (typeof(bool).Equals(type))
            {
                value = bool.Parse(rawString);
            }
            else if (typeof(byte).Equals(type))
            {
                value = byte.Parse(rawString, CultureInfo.InvariantCulture);
            }
            else if (typeof(char).Equals(type))
            {
                value = rawString.FirstOrDefault();
            }
            else if (typeof(DateTime).Equals(type))
            {
                value = DateTime.Parse(rawString, CultureInfo.InvariantCulture);
            }
            else if (typeof(decimal).Equals(type))
            {
                value = decimal.Parse(rawString, CultureInfo.InvariantCulture);
            }
            else if (typeof(double).Equals(type))
            {
                value = double.Parse(rawString, CultureInfo.InvariantCulture);
            }
            else if (typeof(short).Equals(type))
            {
                value = short.Parse(rawString, CultureInfo.InvariantCulture);
            }
            else if (typeof(int).Equals(type))
            {
                value = int.Parse(rawString, CultureInfo.InvariantCulture);
            }
            else if (typeof(long).Equals(type))
            {
                value = long.Parse(rawString, CultureInfo.InvariantCulture);
            }
            else if (typeof(ushort).Equals(type))
            {
                value = ushort.Parse(rawString, CultureInfo.InvariantCulture);
            }
            else if (typeof(uint).Equals(type))
            {
                value = uint.Parse(rawString, CultureInfo.InvariantCulture);
            }
            else if (typeof(ulong).Equals(type))
            {
                value = ulong.Parse(rawString, CultureInfo.InvariantCulture);
            }
            else if (typeof(float).Equals(type))
            {
                value = float.Parse(rawString, CultureInfo.InvariantCulture);
            }
            else if (typeof(string).Equals(type))
            {
                value = rawString;
            }
            return value;
        }
    }
}