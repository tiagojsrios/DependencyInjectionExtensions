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

                INamedTypeSymbol typePropertyAsTypeSymbol = type.GetAttributeByName(ServiceDescriptorAttributeName)
                    .GetNamedArgument<INamedTypeSymbol>("Type");

                services.Add(new ServiceProxyModel(type.GetAssemblyName(),
                    type.ToString(),
                    typePropertyAsTypeSymbol?.ToString(),
                    serviceLifetime,
                    typePropertyAsTypeSymbol?.IsUnboundGenericType ?? ((INamedTypeSymbol)type).IsGenericType));
            }

            context.AddSource("ServiceCollectionExtensions.generated.cs", GenerateServiceCollectionExtensionsClass(services));
        }

        private static SourceText GenerateServiceCollectionExtensionsClass(IEnumerable<ServiceProxyModel> services)
        {
            var servicesGroupedByNamespace = services.GroupBy(x => x.AssemblyNamespace);

            StringBuilder stringBuilder = new();

            foreach (var group in servicesGroupedByNamespace)
            {
                string stringResult =
$@"using Microsoft.Extensions.DependencyInjection;

namespace {group.Key}.Extensions
{{
    public static partial class ServiceCollectionExtensions
    {{
        internal static IServiceCollection RegisterServices(this IServiceCollection services) => services.RegisterServicesFor{group.Key.ToSafeIdentifier()}();
        
        public static IServiceCollection RegisterServicesFor{group.Key.ToSafeIdentifier()}(this IServiceCollection services)
        {{
            {GetMethodBody(group.ToList())}

            return services;
        }}
    }}
}}

";
                stringBuilder.Append(stringResult);
            }

            return SyntaxFactory.ParseCompilationUnit(stringBuilder.ToString())
                .NormalizeWhitespace()
                .GetText(Encoding.UTF8);
        }

        private static string GetMethodBody(IEnumerable<ServiceProxyModel> services)
        {
            List<string> methodLines = services
                .Select(service => service.GetDependencyInjectionEntry())
                .ToList();

            return string.Join("\n", methodLines);
        }
    }
}
