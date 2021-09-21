using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.Serialization;

namespace DependencyInjectionExtensions.Attributes
{
    /// <summary>
    ///     Attribute to be used on Services that should be added to the dependency injection container.
    ///     By default, it registers all interfaces the Service implements
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    internal class ServiceDescriptorAttribute : Attribute
    {
        /// <summary>
        ///     Lifetime of the service where this attribute is being added
        /// </summary>
        public ServiceLifetime ServiceLifetime { get; }

        public RegistrationTypeSelectors RegistrationTypeSelector { get; }

        /// <summary>
        ///     Types that should be registered when manual mode is used
        /// </summary>
        public Type[] Types { get; }

        /// <summary>
        ///     Types that should not be registered
        /// </summary>
        public Type[] ExcludedTypes { get; set; } = { typeof(IDisposable), typeof(IAsyncDisposable), typeof(ISerializable) };

        /// <summary>
        ///     <see cref="ServiceDescriptorAttribute"/> ctor
        /// </summary>
        public ServiceDescriptorAttribute(ServiceLifetime serviceLifetime, RegistrationTypeSelectors selector = RegistrationTypeSelectors.ImplementedInterfaces )
        {
            ServiceLifetime = serviceLifetime;
            RegistrationTypeSelector = selector;
            Types = null;
        }

        /// <summary>
        ///     <see cref="ServiceDescriptorAttribute"/> ctor
        /// </summary>
        public ServiceDescriptorAttribute(ServiceLifetime serviceLifetime, params Type[] types)
        {
            ServiceLifetime = serviceLifetime;
            RegistrationTypeSelector = RegistrationTypeSelectors.Manual;
            Types = types ?? Array.Empty<Type>();
        }
    }

    internal enum RegistrationTypeSelectors
    {
        ImplementedInterfaces,
        SelfAndImplementedInterfaces,
        Manual
    }
}
