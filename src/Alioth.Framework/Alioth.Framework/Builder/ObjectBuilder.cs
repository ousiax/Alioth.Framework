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
    [DebuggerDisplay("Type={objectType}")]
    internal class ObjectBuilder : IObjectBuilder, IAliothServiceProvider {
        private IAliothServiceContainer container;
        private Type objectType;
        private IDictionary<String, String> parameters;
        private IDictionary<String, String> properties;

        [DepedencyAtrribute]
        public ObjectBuilder() {
            this.parameters = new Dictionary<String, String>();
            this.properties = new Dictionary<String, String>();
        }

        public ObjectBuilder(Type objectType) : this() {
            #region precondition
            if (objectType == null) {
                throw new ArgumentNullException("value");
            }
            if (!objectType.IsClass) {
                throw new ArgumentOutOfRangeException("value", "The specified object type should be a concrete class.");
            }
            #endregion
            this.objectType = objectType;
        }

        public IFormatProvider CutureInfo { get; private set; }

        public Type ObjectType {
            get { return objectType; }

            set {
                #region precondition
                if (value == null) {
                    throw new ArgumentNullException("value");
                }
                if (!value.IsClass) {
                    throw new ArgumentOutOfRangeException("value", "The specified object type should be a concrete class.");
                }
                #endregion
                this.objectType = value;
            }
        }

        public IDictionary<String, String> Parameters {
            get { return this.parameters; }
        }

        public IDictionary<String, String> Properties {
            get { return this.properties; }
        }

        public virtual Object Build() {
            ConstructorInfo[] ctors = objectType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (ctors.Length > 1) {
                var ctors2 = ctors.Where(o => o.GetCustomAttributes(false).Any(p => p.GetType() == typeof(DepedencyAtrribute))).ToArray();
                if (ctors2.Length > 1) {
                    throw new InvalidOperationException("Too many constructors that are annotated with Alioth.Framework.DepedencyAtrribute.");
                } else if (ctors2.Length == 0) {
                    throw new InvalidOperationException("Too many constructors but no one is annotated with Alioth.Framework.DepedencyAtrribute.");
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
            InjectContextContainer(instance);
            InjectDepedencyProperties(instance);
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
                    throw new ArgumentException(pi.Name, String.Format("Parameter \"{0}\" should be provided.", pi.Name));
                }
            } else {
                DepedencyAtrribute depAttr = depAttrs[0];
                value = this.GetService(depAttr.ServiceType, depAttr.ServiceName, depAttr.ServiceVersion);
            }

            return value;
        }

        private static Object ParseValue(Type type, string rawString) {
            Object value = null;
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

            return value;
        }

        private void InjectContextContainer(object instance) {
            IAliothServiceContainerConnector conn = instance as IAliothServiceContainerConnector;
            if (conn != null) {
                conn.Connect(this.container);
            }
        }

        private void InjectDepedencyProperties(object instance) {
            PropertyInfo[] properties = objectType.GetProperties(BindingFlags.SetProperty | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.GetCustomAttributes(false).Any(s => s.GetType() == typeof(DepedencyAtrribute)))
                .ToArray();
            foreach (PropertyInfo p in properties) {
                DepedencyAtrribute attr = (DepedencyAtrribute)p.GetCustomAttributes(typeof(DepedencyAtrribute), false)[0];
                var v = this.GetService(attr.ServiceType, attr.ServiceName, attr.ServiceVersion); //TODO consider to issue a warning message when the depedency service was not found.
                p.SetValue(instance, v, null);
            }
        }
    }
}
