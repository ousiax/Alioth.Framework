/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;
using System.Collections.Concurrent;
using System.Linq;
using Alioth.Framework.Builder;

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

        public IAliothServiceContainer Apply(Type objectType, string name, string version) {
            #region precondition
            if (objectType == null) { throw new ArgumentNullException("objectType"); }
            #endregion
            ServiceTypeAtrribute[] attributes = (ServiceTypeAtrribute[])objectType.GetCustomAttributes(typeof(ServiceTypeAtrribute), false);
            if (attributes.Length == 0) {
                throw new ArgumentException(String.Format("{0} should be to anotate with {1}", objectType.Name, attributes));
            }
            IObjectBuilder builder = GetBuilder(objectType, attributes);
            foreach (ServiceTypeAtrribute attribute in attributes) {
                ServiceKey key = ServiceKey.Create(attribute.ServiceType, name, version);
                AddBuilder(key, builder);
            }
            return this;
        }

        private static IObjectBuilder GetBuilder(Type objectType, ServiceTypeAtrribute[] attrites) {
            Boolean isSingleton = attrites.Any(o => o.ReferenceType == ReferenceType.Singleton);
            IObjectBuilder builder;
            if (isSingleton) {
                builder = new SingletonBuilder(objectType);
            } else {
                builder = new ObjectBuilder(objectType);
            }
            return builder;
        }

        public IAliothServiceContainer Apply<T, O>(O instance, string name, string version) where O : T {
            var key = new ServiceKey(typeof(T), name, version);
            var builder = SingletonBuilder.Create(instance);
            AddBuilder(key, builder);
            return this;
        }

        private void AddBuilder(ServiceKey key, IObjectBuilder builder) {
            if (!builderContainer.TryAdd(key, builder)) {
                throw new ArgumentException(String.Format("An element with the same key:\"{0}\", already exists in the AliothServiceContainer:\"{1}\"", key, Description));
            }
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
    }
}
