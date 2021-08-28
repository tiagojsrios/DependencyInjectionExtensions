using DependencyInjectionExtensions.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
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
        ///     Tuple with type's name and flag whether type is generic or not
        /// </summary>
        public (string Name, bool IsGenericType) TypeInformation { get; set; }

        /// <summary>
        ///     Collection of tuples with interface's name and flag whether interface is generic or not
        /// </summary>
        public IEnumerable<(string Name, bool IsGenericType)> InterfaceImplementations { get; set; }

        /// <summary>
        ///     Lifetime the service should have into the Dependency Injection
        /// </summary>
        public ServiceLifetime ServiceLifetime { get; set; }

        /// <summary>
        ///     <see cref="ServiceProxyModel"/> ctor
        /// </summary>
        public ServiceProxyModel(string assemblyNamespace, (string name, bool isGenericType) typeInformation,
            IEnumerable<(string name, bool isGenericType)> interfaceImplementations, ServiceLifetime serviceLifetime)
        {
            AssemblyNamespace = assemblyNamespace;
            TypeInformation = typeInformation;
            InterfaceImplementations = interfaceImplementations;
            ServiceLifetime = serviceLifetime;
        }

        /// <summary>
        ///     Creates the string to be appended to the generated <see cref="IServiceCollection"/> extension method
        /// </summary>
        public string GetDependencyInjectionEntry()
        {
            bool isInterfaceDeclared = InterfaceImplementations.Any();

            List<string> typeDeclaration = new();

            if (isInterfaceDeclared)
            {
                foreach (var (interfaceName, isInterfaceGeneric) in InterfaceImplementations)
                {
                    bool isOpenGeneric = TypeInformation.IsGenericType && isInterfaceGeneric;

                    typeDeclaration.Add(
                        $"services.TryAdd{ServiceLifetime}{(isOpenGeneric ? $"(typeof({interfaceName.Replace("<T>", "<>")}), typeof({TypeInformation.Name.Replace("<T>", "<>")}));" : $"<{interfaceName}, {TypeInformation.Name}>();")}"
                    );
                }
            }
            else
            {
                typeDeclaration.Add($"services.TryAdd{ServiceLifetime}{(TypeInformation.IsGenericType ? $"(typeof({TypeInformation.Name.Replace("<T>", "<>")}));" : $"<{TypeInformation.Name}>();")}");
            }

            return new StringBuilder()
                .AppendMultiple(typeDeclaration)
                .AppendLine()
                .ToString();
        }
    }
}
