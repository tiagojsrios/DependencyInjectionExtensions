using System;

namespace DependencyInjectionExtensions.Attributes
{
    /// <summary>
    ///     Attribute to be used on Services that should be added to the dependency injection container
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceDescriptorAttribute : Attribute
    {
        /// <summary>
        ///     Lifetime of the service where this attribute is being added
        /// </summary>
        public string ServiceLifetime { get; }

        /// <summary>
        ///     Interface implementation of the service
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        ///     <see cref="ServiceDescriptorAttribute"/> ctor
        /// </summary>
        /// <param name="serviceLifetime"></param>
        public ServiceDescriptorAttribute(string serviceLifetime)
        {
            ServiceLifetime = serviceLifetime;
        }
    }
}
