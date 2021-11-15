using DependencyInjectionExtensions.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace DependencyInjectionExtensions.Models
{
    public class OptionsRegistrations
    {
        public string Name { get; set; }

        public string ConfigurationSectionName { get; set; }

        public bool ValidateDataAnnotations { get; set; }

        public OptionsRegistrations(string name, string configurationSectionName, bool validateDataAnnotations)
        {
            Name = name;
            ConfigurationSectionName = configurationSectionName;
            ValidateDataAnnotations = validateDataAnnotations;
        }

        public static string RenderClass(string @namespace, IEnumerable<OptionsRegistrations> options) =>
$@"using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace {@namespace}.Extensions
{{
    public static partial class ServiceCollectionExtensions
    {{
        internal static IServiceCollection RegisterOptions(this IServiceCollection services, IConfiguration configuration) => services.RegisterOptionsFor{@namespace.ToSafeIdentifier()}(configuration);
        
        public static IServiceCollection RegisterOptionsFor{@namespace.ToSafeIdentifier()}(this IServiceCollection services, IConfiguration configuration)
        {{
            {string.Join("\n", options.Select(GetDependencyInjectionEntry))}

            return services;
        }}
    }}
}}";
        /// <summary>
        ///     Creates the string to be appended to the generated <see cref="IServiceCollection"/> extension method
        /// </summary>
        public static string GetDependencyInjectionEntry(OptionsRegistrations options)
        {
            if (options.ValidateDataAnnotations)
            {
                return $"services.AddOptions<{options.Name}>()" +
                    $".Bind(configuration.GetSection(\"{options.ConfigurationSectionName}\"))" +
                    ".ValidateDataAnnotations();";
            }

            return $"services.AddOptions<{options.Name}>()" +
                   $".Bind(configuration.GetSection(\"{options.ConfigurationSectionName}\"));";
        }
    }
}
