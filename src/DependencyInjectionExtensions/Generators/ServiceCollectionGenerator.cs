using DependencyInjectionExtensions.Generators.Models;
using DependencyInjectionExtensions.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DependencyInjectionExtensions.Generators
{
    /// <summary>
    ///     <see cref="ISourceGenerator"/> for <see cref="IServiceCollection"/>.
    ///     Generates an extension method that will add Services and Repositories to the Dependency Injection container
    /// </summary>
    [Generator]
    public class ServiceCollectionGenerator : ISourceGenerator
    {
        public const string ServiceDescriptorAttributeName = "ServiceDescriptorAttribute";

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForPostInitialization(ctx => ctx.AddSource($"{ServiceDescriptorAttributeName}.generated.cs",
                EmbeddedResourceHelper.GetEmbeddedResource($"Attributes.{ServiceDescriptorAttributeName}.cs")));

            context.RegisterForSyntaxNotifications(() => new AttributeSyntaxReceiver(ServiceDescriptorAttributeName));
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not AttributeSyntaxReceiver syntaxReceiver) { return; }

            List<ServiceProxyModel> services = new();

            foreach (var candidateTypeNode in syntaxReceiver.Candidates)
            {
                SemanticModel model = context.Compilation.GetSemanticModel(candidateTypeNode.SyntaxTree);
                ITypeSymbol type = ModelExtensions.GetDeclaredSymbol(model, candidateTypeNode) as ITypeSymbol;

                if (type == null) { continue; }

                ServiceLifetime serviceLifetime = type.GetAttributeByName(ServiceDescriptorAttributeName)
                    .GetConstructorArgument<ServiceLifetime>();

                services.Add(new ServiceProxyModel(type.GetAssemblyName(),
                    (type.ToString(), ((INamedTypeSymbol)type).IsGenericType),
                    GetInterfaceImplementations(type),
                    serviceLifetime));
            }

            context.AddSource("ServiceCollectionExtensions.generated.cs", GenerateServiceCollectionExtensionsClass(services));
        }

        private static IEnumerable<(string @interface, bool isOpenGeneric)> GetInterfaceImplementations(ITypeSymbol attributeAsTypeSymbol)
        {
            IEnumerable<(string @interface, bool isOpenGeneric)> typesToRegister = attributeAsTypeSymbol
                .AllInterfaces
                .Select(x => (x.ToString(), x.IsGenericType));

            List<INamedTypeSymbol> excludedTypes = attributeAsTypeSymbol
                .GetAttributeByName(ServiceDescriptorAttributeName)
                .GetArrayNamedArgument<INamedTypeSymbol>("ExcludedTypes")?
                .ToList();

            if (excludedTypes is { Count: > 0 })
            {
                typesToRegister = typesToRegister.Except(excludedTypes.Select(x => (x?.ToString(), x?.IsUnboundGenericType ?? false)));
            }

            return typesToRegister;
        }

        private static SourceText GenerateServiceCollectionExtensionsClass(IEnumerable<ServiceProxyModel> services)
        {
            var servicesGroupedByNamespace = services.GroupBy(x => x.AssemblyNamespace);

            StringBuilder stringBuilder = new();

            foreach (var group in servicesGroupedByNamespace)
            {
                stringBuilder.Append(GetClass(group.Key, group));
            }

            return SyntaxFactory.ParseCompilationUnit(stringBuilder.ToString())
                .NormalizeWhitespace()
                .GetText(Encoding.UTF8);
        }

        private static string GetClass(string @namespace, IEnumerable<ServiceProxyModel> services) =>
$@"using Microsoft.Extensions.DependencyInjection;

namespace {@namespace}.Extensions
{{
    public static partial class ServiceCollectionExtensions
    {{
        internal static IServiceCollection RegisterServices(this IServiceCollection services) => services.RegisterServicesFor{@namespace.ToSafeIdentifier()}();
        
        public static IServiceCollection RegisterServicesFor{@namespace.ToSafeIdentifier()}(this IServiceCollection services)
        {{
            {GetMethodBody(services)}

            return services;
        }}
    }}
}}";

        private static string GetMethodBody(IEnumerable<ServiceProxyModel> services)
            => string.Join("\n", services.Select(s => s.GetDependencyInjectionEntry()));
    }
}
