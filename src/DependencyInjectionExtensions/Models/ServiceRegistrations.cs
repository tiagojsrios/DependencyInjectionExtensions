using DependencyInjectionExtensions.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DependencyInjectionExtensions.Models
{
    /// <summary>
    ///     Proxy model that represents a service to be added into the Dependency Injection container
    /// </summary>
    public class ServiceRegistrations
    {
        /// <summary>
        ///     Implementation type's name and whether it's an open generic or not
        /// </summary>
        public (string Type, bool IsOpenGeneric) ImplementationType { get; set; }

        /// <summary>
        ///     Collection of tuples with the registration lifetime, type's name and flag whether it's an open generic registration or not
        /// </summary>
        public List<(ServiceLifetime Lifetime, string Type, bool IsOpenGeneric)> Registrations { get; set; }

        /// <summary>
        ///     <see cref="ServiceRegistrations"/> ctor
        /// </summary>
        public ServiceRegistrations(
            (string Type, bool IsOpenGeneric) implementationType,
            List<(ServiceLifetime Lifetime, string Type, bool IsOpenGeneric)> registrations)
        {
            ImplementationType = implementationType;
            Registrations = registrations;
        }

        public static string RenderClass(string @namespace, IEnumerable<ServiceRegistrations> services) =>
$@"using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace {@namespace}.Extensions
{{
    public static partial class ServiceCollectionExtensions
    {{
        internal static IServiceCollection RegisterServices(this IServiceCollection services) => services.RegisterServicesFor{@namespace.ToSafeIdentifier()}();
        
        public static IServiceCollection RegisterServicesFor{@namespace.ToSafeIdentifier()}(this IServiceCollection services)
        {{
            {string.Join("\n", services.Select(GetDependencyInjectionEntry))}

            return services;
        }}
    }}
}}";

        /// <summary>
        ///     Creates the string to be appended to the generated <see cref="IServiceCollection"/> extension method
        /// </summary>
        public static string GetDependencyInjectionEntry(ServiceRegistrations service)
        {
            List<string> typeDeclaration = new List<string>();

            foreach (var registration in service.Registrations)
            {
                bool isForwardingRegistration = registration.Lifetime != ServiceLifetime.Transient && registration != service.Registrations.FirstOrDefault(r => r.Lifetime == registration.Lifetime);

                if (registration.IsOpenGeneric && isForwardingRegistration) { throw new InvalidOperationException("Open generic registrations with forwarding are not supported."); }
                if (service.ImplementationType.IsOpenGeneric && !registration.IsOpenGeneric) { throw new InvalidOperationException("Open generic implementation types require the registration type to also be an open generic."); }

                if (isForwardingRegistration)
                {
                    typeDeclaration.Add($"services.TryAdd{registration.Lifetime}(typeof({registration.Type}), sp => sp.GetService(typeof({service.Registrations.First(r => r.Lifetime == registration.Lifetime).Type})));");
                }
                else if (registration.IsOpenGeneric)
                {
                    typeDeclaration.Add($"services.TryAdd{registration.Lifetime}(typeof({registration.Type}), typeof({service.ImplementationType.Type}));");
                }
                else
                {
                    typeDeclaration.Add($"services.TryAdd{registration.Lifetime}<{registration.Type}, {service.ImplementationType.Type}>();");
                }
            }

            return string.Join(Environment.NewLine, typeDeclaration);
        }
    }
}
