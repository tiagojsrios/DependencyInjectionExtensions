using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DependencyInjectionExtensions.Helpers
{
    internal static class EmbeddedResourceHelper
    {
        public static string GetEmbeddedResource(string resourceName) => GetEmbeddedResource(Assembly.GetCallingAssembly(), resourceName);

        public static string GetEmbeddedResource(Assembly assembly, string resourceName)
        {
            string fullResourceName = assembly.GetManifestResourceNames()
                                          .Where(x => x.EndsWith(resourceName))
                                          .OrderBy(x => x == resourceName ? -1 : x.EndsWith($".{resourceName}") ? 0 : 1)
                                          .FirstOrDefault()
                                      ?? throw new InvalidOperationException($"Resource '{resourceName}' could not be found");

            using var reader = new StreamReader(assembly.GetManifestResourceStream(fullResourceName)!);
            return reader.ReadToEnd();
        }
    }
}