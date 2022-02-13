using DependencyInjectionExtensions.Helpers;
using DependencyInjectionExtensions.Models;
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
    ///     Generates extension methods that will handle Dependency Injection registration in the <see cref="IServiceCollection"/> container
    /// </summary>
    [Generator]
    public class ServiceDescriptorGenerator : ISourceGenerator
    {
        public const string ServiceDescriptorAttribute = nameof(ServiceDescriptorAttribute);

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForPostInitialization(ctx => ctx.AddSource($"{ServiceDescriptorAttribute}.generated.cs", EmbeddedResourceHelper.GetEmbeddedResource($"{ServiceDescriptorAttribute}.cs")));
            context.RegisterForSyntaxNotifications(() => new AttributeSyntaxReceiver(false, ServiceDescriptorAttribute));
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not AttributeSyntaxReceiver syntaxReceiver) { return; }

            List<ServiceRegistrations> services = new List<ServiceRegistrations>();

            foreach (var candidateTypeNode in syntaxReceiver.Candidates)
            {
                SemanticModel model = context.Compilation.GetSemanticModel(candidateTypeNode.SyntaxTree);
                INamedTypeSymbol type = ModelExtensions.GetDeclaredSymbol(model, candidateTypeNode) as INamedTypeSymbol;

                if (type == null) { continue; }

                var typeRegistrations = new List<(ServiceLifetime Lifetime, INamedTypeSymbol RegistrationType)>();

                IEnumerable<AttributeData> attributes = type.GetAttributes($"DependencyInjectionExtensions.Attributes.{ServiceDescriptorAttribute}");
                foreach (AttributeData attribute in attributes)
                {
                    ServiceLifetime serviceLifetime = attribute.GetConstructorArgument<ServiceLifetime>(0);
                    IEnumerable<INamedTypeSymbol> excludedTypes = attribute.GetArrayNamedArgument<INamedTypeSymbol>("ExcludedTypes");
                    IEnumerable<INamedTypeSymbol> registeredTypes = attribute.GetArrayConstructorArgument<INamedTypeSymbol>(1);

                    if (registeredTypes == null)
                    {
                        int registrationType = attribute.GetConstructorArgument<int>(1);

                        registeredTypes = registrationType switch
                        {
                            0 => type.AllInterfaces, // ImplementedInterfaces
                            1 => type.AllInterfaces.Insert(0, type), // SelfAndImplementedInterfaces
                            _ => Enumerable.Empty<INamedTypeSymbol>() // Manual
                        };
                    }

                    registeredTypes = excludedTypes != null
                        ? registeredTypes.Except(excludedTypes)
                        : registeredTypes.Where(t => t.Name is not ("IDisposable" or "IAsyncDisposable" or "ISerializable"));

                    typeRegistrations.AddRange(registeredTypes.Select(t => (serviceLifetime, t.GetClosedTypeOrOpenGeneric())));
                }

                var implementationType = type.GetClosedTypeOrOpenGeneric();
                services.Add(new ServiceRegistrations(
                    (implementationType.ToDisplayString(), implementationType.IsUnboundGenericType),
                    typeRegistrations.Select(x => (x.Lifetime, x.RegistrationType.ToDisplayString(), x.RegistrationType.IsUnboundGenericType)).ToList()
                    )
                );
            }

            context.AddSource("ServiceCollectionExtensions.generated.cs", GenerateServiceCollectionExtensionsClass(context.Compilation.AssemblyName, services));
        }

        private static SourceText GenerateServiceCollectionExtensionsClass(string @namespace, IEnumerable<ServiceRegistrations> services)
        {
            return SyntaxFactory
                .ParseCompilationUnit(ServiceRegistrations.RenderClass(@namespace, services))
                .NormalizeWhitespace()
                .GetText(Encoding.UTF8);
        }
    }
}
