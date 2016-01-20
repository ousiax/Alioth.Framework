/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;
using System.IO;
using System.Reflection;
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
                try {
                    Type type = Type.GetType(dep.Type);
                    container.Apply(type, dep.Parameters, dep.Properties, dep.Name, dep.Version);
                } catch (TargetInvocationException tie) {
                    throw new Exception(String.Format("Type load failure, '{0}'", dep.Type), tie);
                } catch (ArgumentException ae) {
                    throw new Exception(String.Format("Type load failure, '{0}'", dep.Type), ae);
                } catch (TypeLoadException tle) {
                    throw new Exception(String.Format("Type load failure, '{0}'", dep.Type), tle);
                } catch (BadImageFormatException bfe) {
                    throw new Exception(String.Format("Type load failure, '{0}'", dep.Type), bfe);
                }
            }
            return container;
        }
    }
}
