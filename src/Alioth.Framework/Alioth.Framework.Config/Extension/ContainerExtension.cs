/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;
using System.IO;
using Alioth.Framework.Config;
using Newtonsoft.Json;

namespace Alioth.Framework {

    public static class ContainerExtension {

        public static IAliothServiceContainer Apply(this IAliothServiceContainer container, String path) {
            #region precondition
            if (path == null) {
                throw new ArgumentNullException("path");
            }
            if (!File.Exists(path)) {
                throw new FileNotFoundException("ContainerExtension.Apply: configuration file not be found.", path);
            }
            #endregion
            using (StreamReader reader = File.OpenText(path)) {
                container.Apply(reader);
            }
            return container;
        }


        public static IAliothServiceContainer Apply(this IAliothServiceContainer container, Stream stream) {
            #region precondition
            if (stream == null) {
                throw new ArgumentNullException("stream");
            }
            #endregion
            return container.Apply(new StreamReader(stream));
        }

        public static IAliothServiceContainer Apply(this IAliothServiceContainer container, StreamReader reader) {
            #region precondition
            if (reader == null) {
                throw new ArgumentNullException("path");
            }
            #endregion
            var json = reader.ReadToEnd();
            var serviceContainer = JsonConvert.DeserializeObject<ServiceContainer>(json);
            foreach (var dep in serviceContainer.Services) {
                Type type = Type.GetType(dep.Type); //TODO issue a warning message about System.Reflection.TargetInvocationException ...
                container.Apply(type, dep.Parameters, dep.Properties, dep.Name, dep.Version);
            }
            return container;
        }
    }
}
