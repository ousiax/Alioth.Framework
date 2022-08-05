/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;

namespace Alioth.Framework
{
    /// <summary>
    /// Represents a singleton object builder.
    /// </summary>
    internal class SingletonBuilder : ObjectBuilder
    {
        private object _instance;
        private readonly object _lockObj = new object();

        /// <summary>
        /// Initializes a new instance of the class <c>Alioth.Framework.SingletonBuilder</c>.
        /// </summary>
        public SingletonBuilder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the class <c>Alioth.Framework.SingletonBuilder</c>.
        /// </summary>
        public SingletonBuilder(Type objectType) : base(objectType)
        {
        }

        /// <summary>
        /// Creates a new instance of the class <c>Alioth.Framework.SingletonBuilder</c>.
        /// </summary>
        /// <param name="instance">A singleton service object.</param>
        /// <returns><c>Alioth.Framework.SingletonBuilder</c>.</returns>
        public static SingletonBuilder Create(object instance)
        {
            if (instance == null) { throw new ArgumentNullException(nameof(instance)); }
            var builder = new SingletonBuilder(instance.GetType())
            {
                _instance = instance
            };
            return builder;
        }

        /// <summary>
        /// Gets or builds the singleton instance of the service object.
        /// </summary>
        /// <returns>An object instance of the service object class.</returns>
        public override object Build()
        {
            if (_instance == null)
            {
                lock (_lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = base.Build();
                    }
                }
            }
            return _instance;
        }
    }
}
