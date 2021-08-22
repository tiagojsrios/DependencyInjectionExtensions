using DependencyInjectionExtensions.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DependencyInjectionExtensions.Generators.Models;

namespace DependencyInjectionExtensions.Generators
{
    /// <summary>
    /// 
    /// </summary>
    [Generator]
    public class ServiceCollectionGenerator : ISourceGenerator
    {
        public const string ServiceDescriptorAttribute = "ServiceDescriptorAttribute";

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForPostInitialization(ctx => ctx.AddSource($"{ServiceDescriptorAttribute}.generated.cs", 
                EmbeddedResourceHelper.GetEmbeddedResource($"Attributes.{ServiceDescriptorAttribute}.cs")));

            context.RegisterForSyntaxNotifications(() => new AttributeSyntaxReceiver(ServiceDescriptorAttribute));
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

                string serviceLifetime = type.GetAttributeByName(ServiceDescriptorAttribute)
                    .GetConstructorArgument<string>();

                INamedTypeSymbol typePropertyAsTypeSymbol = type.GetAttributeByName(ServiceDescriptorAttribute)
                    .GetNamedArgument<INamedTypeSymbol>("Type");

                services.Add(new ServiceProxyModel(type.GetAssemblyName() + ".Extensions", 
                    type.ToString(), 
                    typePropertyAsTypeSymbol?.ToString(), 
                    serviceLifetime,
                    typePropertyAsTypeSymbol?.IsUnboundGenericType ?? ((INamedTypeSymbol)type).IsGenericType));
            }

            context.AddSource("ServiceCollectionExtensions.generated.cs", GenerateServiceCollectionExtensionsClass(services));
        }

        private static SourceText GenerateServiceCollectionExtensionsClass(IEnumerable<ServiceProxyModel> services)
        {
            var servicesGroupedByNamespace = services.GroupBy(x => x.Namespace);

            StringBuilder stringBuilder = new();

            foreach (var group in servicesGroupedByNamespace)
            {
                string stringResult = 
$@"using Microsoft.Extensions.DependencyInjection;

namespace {group.Key}
{{
    public static partial class ServiceCollectionExtensions
    {{
        public static IServiceCollection AddDomainServicesAndRepositories(this IServiceCollection services)
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
