using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace DependencyInjectionExtensions.Generators.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceProxyModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string InterfaceName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ServiceLifetime ServiceLifetime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsOpenGeneric { get; set; }

        /// <summary>
        ///     <see cref="ServiceProxyModel"/> ctor
        /// </summary>
        /// <param name="namespace"></param>
        /// <param name="className"></param>
        /// <param name="interfaceName"></param>
        /// <param name="serviceLifetime"></param>
        /// <param name="isOpenGeneric"></param>
        public ServiceProxyModel(string @namespace, string className, string interfaceName, ServiceLifetime serviceLifetime, bool isOpenGeneric)
        {
            Namespace = @namespace;
            ClassName = className;
            InterfaceName = interfaceName;
            ServiceLifetime = serviceLifetime;
            IsOpenGeneric = isOpenGeneric;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetDependencyInjectionEntry()
        {
            bool isInterfaceDeclared = !string.IsNullOrEmpty(InterfaceName);

            StringBuilder stringBuilder = new($"services.Add{ServiceLifetime}");

            string asdad = isInterfaceDeclared
                ? $"{(IsOpenGeneric ? $"typeof({InterfaceName.Replace("<T>", "<>")}), typeof({ClassName.Replace("<T>", "<>")})" : $"{InterfaceName}, {ClassName}")}"
                : $"{(IsOpenGeneric ? $"typeof({ClassName.Replace("<T>", "<>")})" : $"{ClassName}")}";

            return stringBuilder
                .Append(IsOpenGeneric ? "(" : "<")
                .Append(asdad)
                .Append(IsOpenGeneric ? ");" : ">();")
                .AppendLine()
                .ToString();
        }
    }
}
