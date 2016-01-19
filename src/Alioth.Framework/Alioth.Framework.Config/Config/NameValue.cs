/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;

namespace Alioth.Framework.Config {
    [Serializable]
    public sealed class NameValue {
        public String Name { get; set; }

        public String Value { get; set; }
    }
}
