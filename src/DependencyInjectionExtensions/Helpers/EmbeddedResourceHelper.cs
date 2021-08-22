using System;
using System.IO;
using System.Reflection;

namespace DependencyInjectionExtensions.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    internal static class EmbeddedResourceHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
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
