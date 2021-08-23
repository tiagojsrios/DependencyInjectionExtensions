using System;
using System.IO;
using System.Reflection;

namespace DependencyInjectionExtensions.Helpers
{
    /// <summary>
    ///     Helper class to work with embedded resources
    /// </summary>
    internal static class EmbeddedResourceHelper
    {
        /// <summary>
        ///     Gets the content of an embedded resource based on <paramref name="resourceName"/>
        /// </summary>
        public static string GetEmbeddedResource(string resourceName)
        {
            var assembly = Assembly.GetCallingAssembly();
            resourceName = $"{assembly.GetName().Name}.{resourceName}";

            Stream stream = assembly.GetManifestResourceStream(resourceName)
                            ?? throw new ArgumentException("Resource could not be found", nameof(resourceName));
            using StreamReader reader = new(stream);
            return reader.ReadToEnd();
        }
    }
}
