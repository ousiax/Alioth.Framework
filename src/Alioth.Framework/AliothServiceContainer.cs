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
using System.Reflection;

namespace Alioth.Framework
{
    /// <summary>
    /// Represents a IoC container.
    /// </summary>
    public sealed class AliothServiceContainer : IAliothServiceContainer
    {
        private readonly ConcurrentDictionary<ServiceKey, IObjectBuilder> _builderContainer;

        /// <summary>
        /// Gets or sets the description of the IoC container.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets the parent IoC cotnainer.
        /// </summary>
        public IAliothServiceContainer Parent { get; }

        /// <summary>
        /// Initializes a new instance of the class <c>Alioth.Framework.AliothServiceContainer</c> with a specified parent IoC container <paramref name="parent"/>.
        /// </summary>
        /// <param name="parent">The parent IoC cotnainer of the new IoC container.</param>
        public AliothServiceContainer(IAliothServiceContainer parent = null)
        {
            _builderContainer = new ConcurrentDictionary<ServiceKey, IObjectBuilder>();
            Parent = parent;
        }

        /// <summary>
        /// Applys a service object type that used to create service objects.
        /// </summary>
        /// <param name="objectType">A <c>System.Type</c> that specifies the service object type to create service object.</param>
        /// <param name="parameters">A <c>IDictionary&lt;String, String^gt;</c> that specifies a dictionary of the dependency constructor of the service object type.</param>
        /// <param name="properties">A <c>IDictionary&lt;String, String^gt;</c> specifies a dictionary to set properties of the service object.</param>
        /// <param name="name">An <c>System.String</c> that specifies the name of service object to get.</param>
        /// <param name="version">An <c>System.String</c> that specifies the version of service object to get.</param>
        /// <returns>An IoC container that implements <c>Alioth.Framework.IAliothServiceContainer</c>.</returns>
        public IAliothServiceContainer Apply(Type objectType, IDictionary<string, string> parameters, IDictionary<string, string> properties, string name, string version)
        {
            if (objectType == null) { throw new ArgumentNullException(nameof(objectType)); }

            if (!objectType.GetTypeInfo().IsClass)
            {
                throw new ArgumentOutOfRangeException(nameof(objectType), "The specified object type should be a concrete class.");
            }

            var attributes = objectType.GetTypeInfo().GetCustomAttributes<ServiceTypeAttribute>(false).ToArray();
            if (attributes.Length == 0)
            {
                throw new ArgumentException("{objectType.Name} should be to anotate with {attributes}");
            }
            var builder = GetBuilder(objectType, parameters, properties, attributes);
            foreach (var attribute in attributes)
            {
                var key = ServiceKey.Create(attribute.ServiceType, name, version);
                AddBuilder(key, builder);
            }
            return this;
        }

        /// <summary>
        /// Applys a singleton service object.
        /// </summary>
        /// <typeparam name="T">The service type of the service object.</typeparam>
        /// <typeparam name="TService">The type of the service object.</typeparam>
        /// <param name="instance">The service object.</param>
        /// <param name="name">An <c>System.String</c> that specifies the name of service object to get.</param>
        /// <param name="version">An <c>System.String</c> that specifies the version of service object to get.</param>
        /// <returns>An IoC container that implements <c>Alioth.Framework.IAliothServiceContainer</c>.</returns>
        public IAliothServiceContainer Apply<T, TService>(TService instance, string name, string version) where TService : T
        {
            var key = new ServiceKey(typeof(T), name, version);
            var builder = SingletonBuilder.Create(instance); //TODO create object build with IoC container.
            AddBuilder(key, builder);
            return this;
        }

        /// <summary>
        /// Creates a child IoC container.
        /// </summary>
        /// <param name="description">A <c>System.String</c> that represents the description of the child IoC container.</param>
        /// <returns>An IoC container that implements <c>Alioth.Framework.IAliothServiceContainer</c>.</returns>
        public IAliothServiceContainer CreateChild(string description = null)
        {
            return new AliothServiceContainer(this) { Description = description };
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An <c>System.Type</c> that specifies the type of service object to get.</param>
        /// <returns>A service object of type serviceType.-or- null if there is no service object of type serviceType.</returns>
        public object GetService(Type serviceType)
        {
            object obj = null;

            var key = ServiceKey.Create(serviceType);
            if (_builderContainer.TryGetValue(key, out var builder))
            {
                obj = builder.Build();
            }
            else if (Parent != null)
            {
                obj = Parent.GetService(serviceType);
            }
            return obj;
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An <c>System.Type</c> that specifies the type of service object to get.</param>
        /// <param name="name">An <c>System.String</c> that specifies the name of service object to get.</param>
        /// <param name="version">An <c>System.String</c> that specifies the version of service object to get.</param>
        /// <returns>A service object of type serviceType.-or- null if there is no service object of type serviceType.</returns>
        public object GetService(Type serviceType, string name, string version)
        {
            object obj = null;

            var key = ServiceKey.Create(serviceType, name, version);
            if (_builderContainer.TryGetValue(key, out var builder))
            {
                obj = builder.Build();
            }
            else if (Parent != null)
            {
                obj = Parent.GetService(serviceType);
            }
            return obj;
        }

        private IObjectBuilder GetBuilder(Type objectType, IDictionary<string, string> parameters, IDictionary<string, string> properties, ServiceTypeAttribute[] attributes)
        {
            IObjectBuilder builder;

            var isSingleton = attributes.Any(o => o.ReferenceType == ReferenceType.Singleton);
            if (isSingleton)
            {
                builder = new SingletonBuilder(); //TODO create object build with IoC container.
            }
            else
            {
                builder = new ObjectBuilder(); //TODO create object build with IoC container.
            }
            builder.ObjectType = objectType;
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    builder.Parameters.Add(param.Key, param.Value);
                }
            }
            if (properties != null)
            {
                foreach (var prop in properties)
                {
                    builder.Properties.Add(prop.Key, prop.Value);
                }
            }
            builder.Connect(this);
            return builder;
        }

        private void AddBuilder(ServiceKey key, IObjectBuilder builder)
        {
            if (!_builderContainer.TryAdd(key, builder))
            {
                throw new ArgumentException($"An element with the same key:\"{key}\", already exists in the AliothServiceContainer:\"{Description}\"");
            }
        }
    }
}

