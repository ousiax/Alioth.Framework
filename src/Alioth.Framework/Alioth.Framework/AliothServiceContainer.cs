/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Alioth.Framework {

    public sealed class AliothServiceContainer : IAliothServiceContainer {
        private IAliothServiceContainer parent;
        private ConcurrentDictionary<ServiceKey, IObjectBuilder> builderContainer;

        public String Description { get; set; }

        public IAliothServiceContainer Parent { get { return parent; } }

        public AliothServiceContainer(IAliothServiceContainer parent = null) {
            this.builderContainer = new ConcurrentDictionary<ServiceKey, IObjectBuilder>();
            this.parent = parent;
        }

        public IAliothServiceContainer Apply(Type objectType, IDictionary<String, String> parameters, IDictionary<String, String> properties, string name, string version) {
            #region precondition
            if (objectType == null) { throw new ArgumentNullException("objectType"); }
            if (!objectType.IsClass) {
                throw new ArgumentOutOfRangeException("objectType", "The specified object type should be a concrete class.");
            }
            #endregion
            ServiceTypeAtrribute[] attributes = (ServiceTypeAtrribute[])objectType.GetCustomAttributes(typeof(ServiceTypeAtrribute), false);
            if (attributes.Length == 0) {
                throw new ArgumentException(String.Format("{0} should be to anotate with {1}", objectType.Name, attributes));
            }
            IObjectBuilder builder = GetBuilder(objectType, parameters, properties, attributes);
            foreach (ServiceTypeAtrribute attribute in attributes) {
                ServiceKey key = ServiceKey.Create(attribute.ServiceType, name, version);
                AddBuilder(key, builder);
            }
            return this;
        }

        public IAliothServiceContainer Apply<T, O>(O instance, string name, string version) where O : T {
            var key = new ServiceKey(typeof(T), name, version);
            var builder = SingletonBuilder.Create(instance); //TODO create object build with IoC container.
            AddBuilder(key, builder);
            return this;
        }

        public IAliothServiceContainer CreateChild(string description = null) {
            return new AliothServiceContainer(this) { Description = description };
        }

        public object GetService(Type serviceType) {
            Object obj = null;

            var key = ServiceKey.Create(serviceType);
            IObjectBuilder builder;
            if (builderContainer.TryGetValue(key, out builder)) {
                obj = builder.Build();
            } else if (parent != null) {
                obj = parent.GetService(serviceType);
            }
            return obj;
        }

        public object GetService(Type serviceType, string name, string version) {
            Object obj = null;

            var key = ServiceKey.Create(serviceType, name, version);
            IObjectBuilder builder;
            if (builderContainer.TryGetValue(key, out builder)) {
                obj = builder.Build();
            } else if (parent != null) {
                obj = parent.GetService(serviceType);
            }
            return obj;
        }

        private IObjectBuilder GetBuilder(Type objectType, IDictionary<String, String> parameters, IDictionary<String, String> properties, ServiceTypeAtrribute[] attributes) {
            IObjectBuilder builder;

            Boolean isSingleton = attributes.Any(o => o.ReferenceType == ReferenceType.Singleton);
            if (isSingleton) {
                builder = new SingletonBuilder();
            } else {
                builder = new ObjectBuilder();
            }
            builder.ObjectType = objectType;
            if (parameters != null) {
                foreach (var param in parameters) {
                    builder.Parameters.Add(param.Key, param.Value);
                }
            }
            if (properties != null) {
                foreach (var prop in properties) {
                    builder.Properties.Add(prop.Key, prop.Value);
                }
            }
            builder.Connect(this);
            return builder;
        }

        private void AddBuilder(ServiceKey key, IObjectBuilder builder) {
            if (!builderContainer.TryAdd(key, builder)) {
                throw new ArgumentException(String.Format("An element with the same key:\"{0}\", already exists in the AliothServiceContainer:\"{1}\"", key, Description));
            }
        }
    }
}
