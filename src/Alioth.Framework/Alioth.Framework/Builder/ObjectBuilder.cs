/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;

namespace Alioth.Framework.Builder {
    internal class ObjectBuilder : IObjectBuilder {
        private Type objectType;

        public Type ObejectType { get { return objectType; } }

        public ObjectBuilder(Type objectType) {
            #region precondition
            if (objectType == null) { throw new ArgumentNullException("objectType"); }
            #endregion
            this.objectType = objectType;
        }

        public virtual object Build() {
            throw new NotImplementedException();
        }
    }
}
