using System;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionExtensions.Attributes
{
    /// <summary>
    ///     Attribute to be used on Services that should be added to the dependency injection container.
    ///     By default, it registers all interfaces the Service implements
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ServiceDescriptorAttribute : Attribute
    {
        /// <summary>
        ///     Lifetime of the service where this attribute is being added
        /// </summary>
        public ServiceLifetime ServiceLifetime { get; }

        /// <summary>
        ///     Interface implementation of the service
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        ///     Collection with types that should not be registered
        /// </summary>
        public Type[] ExcludedTypes { get; set; }
        
        /// <summary>
        ///     <see cref="ServiceDescriptorAttribute"/> ctor
        /// </summary>
        public ServiceDescriptorAttribute(ServiceLifetime serviceLifetime)
        {
            ServiceLifetime = serviceLifetime;
        }
    }
}
