using Microsoft.CodeAnalysis;
using System.Linq;

namespace DependencyInjectionExtensions.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class TypeSymbolExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeSymbol"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static AttributeData GetAttributeByName(this ITypeSymbol typeSymbol, string attributeName)
        {
            return typeSymbol.GetAttributes().First(x => x.AttributeClass?.Name == attributeName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="attributeData"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetNamedArgument<T>(this AttributeData attributeData, string name)
        {
            return (T) attributeData.NamedArguments.FirstOrDefault(kp => kp.Key == name).Value.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="attributeData"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T GetConstructorArgument<T>(this AttributeData attributeData, int index = 0)
        {
            return (T) attributeData.ConstructorArguments[index].Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeSymbol"></param>
        /// <returns></returns>
        public static string GetAssemblyName(this ITypeSymbol typeSymbol)
        {
            return typeSymbol.ContainingAssembly.Name;
        }
    }
}
