/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;

namespace Alioth.Framework.Builder {
    internal class SingletonBuilder : ObjectBuilder {
        private Object instance;
        private Object lockObj = new Object();

        public SingletonBuilder(Type objectType) : base(objectType) {
        }

        public static SingletonBuilder Create(Object instance) {
            #region precondition
            if (instance == null) { throw new ArgumentNullException("instance"); }
            #endregion
            SingletonBuilder builder = new SingletonBuilder(instance.GetType());
            builder.instance = instance;
            return builder;
        }

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
