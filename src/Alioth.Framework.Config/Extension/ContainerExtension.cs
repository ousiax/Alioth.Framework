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

namespace Alioth.Framework
{

    /// <summary>
    /// Defines a mechanism for appling service object type to a <c>Alioth.Framework.IAliothServiceContainer</c>.
    /// </summary>
    public static class ContainerExtension
    {

        /// <summary>
        /// Applys a json service metadata configuration file.
        /// </summary>
        /// <param name="container">A <c>Alioth.Framework.IAliothServiceContainer</c> will be applied.</param>
        /// <param name="path">The path of the metadata file.</param>
        /// <returns>A object of <c>Alioth.Framework.IAliothServiceContainer</c>.</returns>
        public static IAliothServiceContainer Apply(this IAliothServiceContainer container, string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (!File.Exists(path))
            {
                throw new ArgumentException("ContainerExtension.Apply: configuration file not be found.", path);
            }
            using (StreamReader reader = File.OpenText(path))
            {
                container.Apply(reader);
            }
            return container;
        }

        /// <summary>
        /// Applys a json service metadata configuration file.
        /// </summary>
        /// <param name="container">A <c>Alioth.Framework.IAliothServiceContainer</c> will be applied.</param>
        /// <param name="stream">A <c>System.IO.Stream</c> that represents a metadata file.</param>
        /// <returns>A object of <c>Alioth.Framework.IAliothServiceContainer</c>.</returns>
        public static IAliothServiceContainer Apply(this IAliothServiceContainer container, Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            return container.Apply(new StreamReader(stream));
        }

        /// <summary>
        /// Applys a json service metadata configuration file.
        /// </summary>
        /// <param name="container">A <c>Alioth.Framework.IAliothServiceContainer</c> will be applied.</param>
        /// <param name="reader">A <c>System.IO.Stream</c> that represents a metadata file.</param>
        /// <returns>A object of <c>Alioth.Framework.IAliothServiceContainer</c>.</returns>
        public static IAliothServiceContainer Apply(this IAliothServiceContainer container, StreamReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }
            var json = reader.ReadToEnd();
            var serviceContainer = JsonConvert.DeserializeObject<ServiceContainer>(json);
            foreach (var dep in serviceContainer.Services)
            {
#if DEBUG
                Type type = Type.GetType(dep.Type);
                container.Apply(type, dep.Parameters, dep.Properties, dep.Name, dep.Version);
#else
                try {
                    Type type = Type.GetType(dep.Type);
                    container.Apply(type, dep.Parameters, dep.Properties, dep.Name, dep.Version);
                } catch (System.Reflection.TargetInvocationException tie) {
                    throw new Exception(String.Format("Type load failure, '{0}'", dep.Type), tie);
                } catch (ArgumentException ae) {
                    throw new Exception(String.Format("Type load failure, '{0}'", dep.Type), ae);
                } catch (TypeLoadException tle) {
                    throw new Exception(String.Format("Type load failure, '{0}'", dep.Type), tle);
                } catch (BadImageFormatException bfe) {
                    throw new Exception(String.Format("Type load failure, '{0}'", dep.Type), bfe);
                }
#endif
            }
            return container;
        }
    }
}
