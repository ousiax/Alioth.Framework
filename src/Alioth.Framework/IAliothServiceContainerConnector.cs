/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/


namespace Alioth.Framework {
    /// <summary>
    /// Defines a mechanism for connecting IoC cotnainer with the interface injection of IoC.
    /// </summary>
    public interface IAliothServiceContainerConnector {
        /// <summary>
        /// Connect a IoC container when service object are built with <c>Alioth.Framework.IObjectBuilder</c>.
        /// </summary>
        /// <param name="container">A <c>Alioth.Framework.IAliothServiceContainer</c> tha specifies a IoC container to be injected.</param>
        void Connect(IAliothServiceContainer container);
    }
}
