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

namespace Alioth.Framework {
    /// <summary>
    /// Represents a service object builder to build service object.
    /// </summary>
    [DebuggerDisplay("Type={objectType}")]
    internal class ObjectBuilder : IObjectBuilder, IAliothServiceProvider {
        private IAliothServiceContainer container;
        private Type objectType;
        private IDictionary<String, String> parameters;
        private IDictionary<String, String> properties;

        /// <summary>
        /// Gets or sets the service object class type.
        /// </summary>
        public Type ObjectType {
            get { return objectType; }
            set {
                #region precondition
                if (value == null) {
                    throw new ArgumentNullException("value");
                }
#if NET451
                if (!value.IsClass) {
#elif DOTNET5_4
                if (!value.GetTypeInfo().IsClass) {
#endif
                    throw new ArgumentOutOfRangeException("value", "The specified object type should be a concrete class.");
                }
                #endregion
                this.objectType = value;
            }
        }

        /// <summary>
        /// Gets the parameters dictioinary of the service object.
        /// </summary>
        public IDictionary<String, String> Parameters {
            get { return this.parameters; }
        }

        /// <summary>
        /// Gets the properties dictioinary of the service object.
        /// </summary>
        public IDictionary<String, String> Properties {
            get { return this.properties; }
        }

        /// <summary>
        /// Initializes a new instance of the class <c>Alioth.Framework.ObjectBuilder</c>.
        /// </summary>
        public ObjectBuilder() {
            this.parameters = new Dictionary<String, String>();
            this.properties = new Dictionary<String, String>();
        }

        /// <summary>
        /// Initializes a new instance of the class <c>Alioth.Framework.ObjectBuilder</c>.
        /// </summary>
        /// <param name="objectType">The service object class type.</param>
        public ObjectBuilder(Type objectType) : this() {
            #region precondition
            if (objectType == null) {
                throw new ArgumentNullException("value");
            }
#if NET451
            if (!objectType.IsClass) {
#elif DOTNET5_4
            if (!objectType.GetTypeInfo().IsClass) {
#endif
                throw new ArgumentOutOfRangeException("value", "The specified object type should be a concrete class.");
            }
            #endregion
            this.objectType = objectType;
        }

        /// <summary>
        /// Builds a new instance of the service object.
        /// </summary>
        /// <returns>An object instance of the service object class.</returns>
        public virtual Object Build() {
            ConstructorInfo[] ctors = objectType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (ctors.Length > 1) {
                var ctors2 = ctors.Where(o => o.GetCustomAttributes(false).Any(p => p.GetType() == typeof(DepedencyAtrribute))).ToArray();
                if (ctors2.Length > 1) {
                    throw new InvalidOperationException(String.Format("Too many constructors annotated with Alioth.Framework.DepedencyAtrribute. Type: {0}", ObjectType.AssemblyQualifiedName));
                } else if (ctors2.Length == 0) {
                    throw new InvalidOperationException(String.Format("Too many constructors but no one is annotated with Alioth.Framework.DepedencyAtrribute. Type: {0}", ObjectType.AssemblyQualifiedName));
                }
                return Create(ctors2[0]);
            } else {
                return Create(ctors[0]);
            }
        }

        public void Connect(IAliothServiceContainer container) {
            #region precondition
            if (container == null) {
                throw new ArgumentNullException("container");
            }
            #endregion
            this.container = container;
        }

        public object GetService(Type serviceType) {
            return container.GetService(serviceType);
        }

        public object GetService(Type serviceType, string name, string version) {
            return container.GetService(serviceType, name, version);
        }

        private Object Create(ConstructorInfo ctor) {
            ParameterInfo[] ps = ctor.GetParameters();
            Object[] args = new Object[ps.Length];
            for (int i = 0; i < ps.Length; i++) {
                ParameterInfo pi = ps[i];
                object value = GetParameterValue(pi);
                args[i] = value;
            }
            Object instance = ctor.Invoke(args);
            ConnectContainer(instance);
            InjectDepedencyProperties(instance);
            InjectProperties(instance);
            return instance;
        }

        private object GetParameterValue(ParameterInfo pi) {
            Object value = null;
            DepedencyAtrribute[] depAttrs = (DepedencyAtrribute[])pi.GetCustomAttributes(typeof(DepedencyAtrribute), false);
            if (depAttrs.Length == 0) {
                Type type = pi.ParameterType;
                String name = pi.Name;
                String s;
                if (parameters.TryGetValue(name, out s)) {
                    value = ParseValue(type, s);
                } else if ((pi.Attributes & ParameterAttributes.HasDefault) == ParameterAttributes.HasDefault) {
                    value = pi.DefaultValue;
                } else {
                    throw new ArgumentException(pi.Name, String.Format("Parameter \"{0}\" must be provided. Type: {1}", pi.Name, ObjectType.AssemblyQualifiedName));
                }
            } else {
                DepedencyAtrribute depAttr = depAttrs[0];
                value = this.GetService(depAttr.ServiceType, depAttr.ServiceName, depAttr.ServiceVersion);
            }

            return value;
        }

        private void ConnectContainer(object instance) {
            IAliothServiceContainerConnector conn = instance as IAliothServiceContainerConnector;
            if (conn != null) {
                conn.Connect(this.container);
            }
        }

        private void InjectDepedencyProperties(object instance) {
#if NET451
            PropertyInfo[] properties = objectType.GetProperties(BindingFlags.SetProperty | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
#elif DOTNET5_4
            PropertyInfo[] properties = objectType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
#endif
                .Where(p => p.GetCustomAttributes(false).Any(s => s.GetType() == typeof(DepedencyAtrribute)))
                .ToArray();
            foreach (PropertyInfo p in properties) {
#if NET451
                DepedencyAtrribute attr = (DepedencyAtrribute)p.GetCustomAttributes(typeof(DepedencyAtrribute), false)[0];
#elif DOTNET5_4
                DepedencyAtrribute attr = p.GetCustomAttributes<DepedencyAtrribute>().First();
#endif
                var v = this.GetService(attr.ServiceType, attr.ServiceName, attr.ServiceVersion);
                if (v == null) {
                    throw new KeyNotFoundException(
                        String.Format(
                            "The sepecified depedency service with a key '{0}' could not be found. Type: {1}",
                            ServiceKey.Create(attr.ServiceType, attr.ServiceName, attr.ServiceVersion), ObjectType.AssemblyQualifiedName));
                }
                p.SetValue(instance, v, null);
            }
        }

        private void InjectProperties(object instance) {
            if (this.Properties.Count > 0) {
                Type t = instance.GetType();
                foreach (var item in this.Properties) {
#if NET451
                    PropertyInfo pi = t.GetProperty(item.Key, BindingFlags.SetProperty | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
#elif DOTNET5_4
                    PropertyInfo pi = t.GetProperty(item.Key, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
#endif
                    if (pi == null) {
                        throw new KeyNotFoundException(String.Format("Invalid [Properites] Configuration: could not found a property with the specified name '{0}'. Type: {1}", item.Key, ObjectType.AssemblyQualifiedName));
                    }
                    pi.SetValue(instance, ParseValue(pi.PropertyType, item.Value), null);
                }
            }
        }

        private static Object ParseValue(Type type, string rawString) {
            Object value = null;
#if NET451
            switch (Type.GetTypeCode(type)) {
                case TypeCode.Boolean:
                    value = Boolean.Parse(rawString);
                    break;
                case TypeCode.Byte:
                    value = Byte.Parse(rawString, CultureInfo.InvariantCulture);
                    break;
                case TypeCode.Char:
                    value = Char.Parse(rawString);
                    break;
                case TypeCode.DateTime:
                    value = DateTime.Parse(rawString, CultureInfo.InvariantCulture);
                    break;
                case TypeCode.Decimal:
                    value = Decimal.Parse(rawString, CultureInfo.InvariantCulture);
                    break;
                case TypeCode.Double:
                    value = Double.Parse(rawString, CultureInfo.InvariantCulture);
                    break;
                case TypeCode.Int16:
                    value = Int16.Parse(rawString, CultureInfo.InvariantCulture);
                    break;
                case TypeCode.Int32:
                    value = Int32.Parse(rawString, CultureInfo.InvariantCulture);
                    break;
                case TypeCode.Int64:
                    value = Int64.Parse(rawString, CultureInfo.InvariantCulture);
                    break;
                case TypeCode.UInt16:
                    value = UInt16.Parse(rawString, CultureInfo.InvariantCulture);
                    break;
                case TypeCode.UInt32:
                    value = UInt32.Parse(rawString, CultureInfo.InvariantCulture);
                    break;
                case TypeCode.UInt64:
                    value = UInt64.Parse(rawString, CultureInfo.InvariantCulture);
                    break;
                case TypeCode.Single:
                    value = Single.Parse(rawString, CultureInfo.InvariantCulture);
                    break;
                case TypeCode.String:
                    value = rawString;
                    break;
            }
#elif DOTNET5_4
            if (typeof(Boolean).Equals(type)) {
                value = Boolean.Parse(rawString);
            } else if (typeof(Byte).Equals(type)) {
                value = Byte.Parse(rawString, CultureInfo.InvariantCulture);
            } else if (typeof(Char).Equals(type)) {
                value = Char.Parse(rawString);
            } else if (typeof(DateTime).Equals(type)) {
                value = DateTime.Parse(rawString, CultureInfo.InvariantCulture);
            } else if (typeof(Decimal).Equals(type)) {
                value = Decimal.Parse(rawString, CultureInfo.InvariantCulture);
            } else if (typeof(Double).Equals(type)) {
                value = Double.Parse(rawString, CultureInfo.InvariantCulture);
            } else if (typeof(Int16).Equals(type)) {
                value = Int16.Parse(rawString, CultureInfo.InvariantCulture);
            } else if (typeof(Int32).Equals(type)) {
                value = Int32.Parse(rawString, CultureInfo.InvariantCulture);
            } else if (typeof(Int64).Equals(type)) {
                value = Int64.Parse(rawString, CultureInfo.InvariantCulture);
            } else if (typeof(UInt16).Equals(type)) {
                value = UInt16.Parse(rawString, CultureInfo.InvariantCulture);
            } else if (typeof(UInt32).Equals(type)) {
                value = UInt32.Parse(rawString, CultureInfo.InvariantCulture);
            } else if (typeof(UInt64).Equals(type)) {
                value = UInt64.Parse(rawString, CultureInfo.InvariantCulture);
            } else if (typeof(Single).Equals(type)) {
                value = Single.Parse(rawString, CultureInfo.InvariantCulture);
            } else if (typeof(String).Equals(type)) {
                value = rawString;
            }
#endif
            return value;
        }
    }
}
