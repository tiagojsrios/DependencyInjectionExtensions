using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace DependencyInjectionExtensions.Generators.Models
{
    /// <summary>
    ///     Proxy model that represents a service to be added into the Dependency Injection container
    /// </summary>
    public class ServiceProxyModel
    {
        /// <summary>
        ///     Assembly namespace where the service is located
        /// </summary>
        public string AssemblyNamespace { get; set; }

        /// <summary>
        ///     Service class name
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        ///     Service interface name
        /// </summary>
        public string InterfaceName { get; set; }

        /// <summary>
        ///     Lifetime the service should have into the Dependency Injection
        /// </summary>
        public ServiceLifetime ServiceLifetime { get; set; }

        /// <summary>
        ///     Is the service an open generic
        /// </summary>
        public bool IsOpenGeneric { get; set; }

        /// <summary>
        ///     <see cref="ServiceProxyModel"/> ctor
        /// </summary>
        public ServiceProxyModel(string assemblyNamespace, string className, string interfaceName, ServiceLifetime serviceLifetime, bool isOpenGeneric)
        {
            AssemblyNamespace = assemblyNamespace;
            ClassName = className;
            InterfaceName = interfaceName;
            ServiceLifetime = serviceLifetime;
            IsOpenGeneric = isOpenGeneric;
        }

        /// <summary>
        ///     Creates the string to be appended to the generated <see cref="IServiceCollection"/> extension method
        /// </summary>
        public string GetDependencyInjectionEntry()
        {
            bool isInterfaceDeclared = !string.IsNullOrEmpty(InterfaceName);

            StringBuilder stringBuilder = new($"services.Add{ServiceLifetime}");

            string typeDeclaration = isInterfaceDeclared
                ? $"{(IsOpenGeneric ? $"typeof({InterfaceName.Replace("<T>", "<>")}), typeof({ClassName.Replace("<T>", "<>")})" : $"{InterfaceName}, {ClassName}")}"
                : $"{(IsOpenGeneric ? $"typeof({ClassName.Replace("<T>", "<>")})" : $"{ClassName}")}";

            return stringBuilder
                .Append(IsOpenGeneric ? "(" : "<")
                .Append(typeDeclaration)
                .Append(IsOpenGeneric ? ");" : ">();")
                .AppendLine()
                .ToString();
        }
    }
}
