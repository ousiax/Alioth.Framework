/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;

namespace Alioth.Framework {
    /// <summary>
    /// Represents a singleton object builder.
    /// </summary>
    internal class SingletonBuilder : ObjectBuilder {
        private Object instance;
        private Object lockObj = new Object();

        /// <summary>
        /// Initializes a new instance of the class <c>Alioth.Framework.SingletonBuilder</c>.
        /// </summary>
        public SingletonBuilder() {
        }

        /// <summary>
        /// Initializes a new instance of the class <c>Alioth.Framework.SingletonBuilder</c>.
        /// </summary>
        public SingletonBuilder(Type objectType) : base(objectType) {
        }

        /// <summary>
        /// Creates a new instance of the class <c>Alioth.Framework.SingletonBuilder</c>.
        /// </summary>
        /// <param name="instance">A singleton service object.</param>
        /// <returns><c>Alioth.Framework.SingletonBuilder</c>.</returns>
        public static SingletonBuilder Create(Object instance) {
            #region precondition
            if (instance == null) { throw new ArgumentNullException("instance"); }
            #endregion
            SingletonBuilder builder = new SingletonBuilder(instance.GetType());
            builder.instance = instance;
            return builder;
        }

        /// <summary>
        /// Gets or builds the singleton instance of the service object.
        /// </summary>
        /// <returns>An object instance of the service object class.</returns>
        public override object Build() {
            if (instance == null) {
                lock (lockObj) {
                    if (instance == null) {
                        instance = base.Build();
                    }
                }
            }
            return instance;
        }
    }
}
