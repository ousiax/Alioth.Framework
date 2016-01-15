/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Alioth.Framework.Builder {
    [DebuggerDisplay("Type={objectType}")]
    internal class ObjectBuilder : IObjectBuilder, IAliothServiceProvider {
        private Type objectType;

        public ObjectBuilder(Type objectType) {
            #region precondition
            if (objectType == null) {
                throw new ArgumentNullException("objectType");
            }
            if (objectType.IsAbstract | objectType.IsInterface) {
                throw new ArgumentOutOfRangeException("objectType", "The specified object type could not be an abstract or an interface.");
            }
            #endregion
            this.objectType = objectType;
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

        public object GetService(Type serviceType) {
            throw new NotImplementedException();
        }

        public object GetService(Type serviceType, string name, string version) {
            throw new NotImplementedException();
        }

        private Object Create(ConstructorInfo ctor) {
            Object instance = ctor.Invoke(null); //TODO support constructor with input parameters.
            PropertyInjection(instance);
            return instance;
        }

        private void PropertyInjection(object instance) {
            PropertyInfo[] properties = objectType.GetProperties(BindingFlags.SetProperty | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.GetCustomAttributes(false).Any(s => s.GetType() == typeof(DepedencyAtrribute)))
                .ToArray();
            foreach (PropertyInfo p in properties) {
                DepedencyAtrribute attr = (DepedencyAtrribute)p.GetCustomAttributes(typeof(DepedencyAtrribute), false)[0];
                var v = this.GetService(attr.ServiceType, attr.ServiceName, attr.ServiceVersion);
                p.SetValue(instance, v, null); //TODO consider to issue a warning message when the depedency service was not found.
            }
        }
    }
}
