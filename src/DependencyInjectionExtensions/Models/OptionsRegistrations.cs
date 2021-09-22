using DependencyInjectionExtensions.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace DependencyInjectionExtensions.Models
{
    public class OptionsRegistrations
    {
        public string Name { get; set; }

        public string ConfigurationSectionName { get; set; }

        public OptionsRegistrations(string name, string configurationSectionName)
        {
            Name = name;
            ConfigurationSectionName = configurationSectionName;
        }

        public static string RenderClass(string @namespace, IEnumerable<OptionsRegistrations> options) =>
$@"using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace {@namespace}.Extensions
{{
    public static partial class ServiceCollectionExtensions
    {{
        internal static IServiceCollection RegisterOptions(this IServiceCollection services) => services.RegisterServicesFor{@namespace.ToSafeIdentifier()}();
        
        public static IServiceCollection RegisterOptionsFor{@namespace.ToSafeIdentifier()}(this IServiceCollection services)
        {{
            {string.Join("\n", options.Select(GetDependencyInjectionEntry))}

            return services;
        }}
    }}
}}";
        /// <summary>
        ///     Creates the string to be appended to the generated <see cref="IServiceCollection"/> extension method
        /// </summary>
        public static string GetDependencyInjectionEntry(OptionsRegistrations service)
        {
            return $"services.AddOptions<{service.Name}>()" +
                   $".Bind(configuration.GetSection({service.ConfigurationSectionName}))" +
                   ".ValidateDataAnnotations();";
        }
    }
}
