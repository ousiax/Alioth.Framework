/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

namespace Alioth.Framework
{
    /// <summary>
    /// Specifies the life cycle of the service object.
    /// </summary>
    public enum ReferenceType
    {
        /// <summary>
        /// Specifies a singlton life cycle that will be a zero or a sole service object in the currecnt IoC container.
        /// </summary>
        Singleton,

        /// <summary>
        /// Specifies a strong life cycle that will be reclarmed by GC.
        /// </summary>
        Strong,

        /// <summary>
        /// Specifies a weakness life cycle that will be reclarmed by GC. (Not implement)
        /// </summary>
        Weak,
    }
}
