using DependencyInjectionExtensions.Generators;
using DependencyInjectionExtensions.Helpers;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DependencyInjectionExtensions.Tests
{
    public class DependencyInjectionGeneratorTest
    {
        [Theory]
        [InlineData("Singleton")]
        [InlineData("Scoped")]
        [InlineData("Transient")]
        public Task SimpleServiceRegistration(string lifetime) => RunEmbeddedResourceTest(
            nameof(SimpleServiceRegistration),
            new Dictionary<string, string>
            {
                ["_Lifetime_"] = lifetime
            }
        );

        [Fact]
        public Task MultipleInterfacesRegistration() => RunEmbeddedResourceTest(nameof(MultipleInterfacesRegistration));

        [Fact]
        public Task ClosedGenericRegistration() => RunEmbeddedResourceTest(nameof(ClosedGenericRegistration));

        [Fact]
        public Task OpenGenericRegistration() => RunEmbeddedResourceTest(nameof(OpenGenericRegistration));

        [Fact]
        public Task MultipleRegistrations() => RunEmbeddedResourceTest(nameof(MultipleRegistrations));
        [Fact]
        public Task ImplementedInterfacesAndSelfRegistration() => RunEmbeddedResourceTest(nameof(ImplementedInterfacesAndSelfRegistration));

        [Fact]
        public Task ManualRegistration() => RunEmbeddedResourceTest(nameof(ManualRegistration));

        [Fact]
        public Task DefaultExcludedTypesRegistration() => RunEmbeddedResourceTest(nameof(DefaultExcludedTypesRegistration));

        [Fact]
        public Task CustomExcludedTypesRegistration() => RunEmbeddedResourceTest(nameof(CustomExcludedTypesRegistration));

        private Task RunEmbeddedResourceTest(string testName, IDictionary<string, string>? placeholders = null)
        {
            string input = EmbeddedResourceHelper.GetEmbeddedResource($"{testName}.Input.cs");
            string expectedResult = EmbeddedResourceHelper.GetEmbeddedResource($"{testName}.Result.cs");

            foreach ((string key, string value) in placeholders ?? Enumerable.Empty<KeyValuePair<string, string>>())
            {
                input = input.Replace(key, value);
                expectedResult = expectedResult.Replace(key, value);
            }

            return new CSharpSourceGeneratorTest<DependencyInjectionGenerator, XUnitVerifier>
            {
                TestState =
                {
                    AdditionalReferences = { "Microsoft.Extensions.DependencyInjection.Abstractions.dll" },
                    Sources = { input },
                    GeneratedSources =
                    {
                        (typeof(DependencyInjectionGenerator), "ServiceDescriptorAttribute.generated.cs", EmbeddedResourceHelper.GetEmbeddedResource(typeof(DependencyInjectionGenerator).Assembly, "ServiceDescriptorAttribute.cs")),
                        (typeof(DependencyInjectionGenerator), "ServiceCollectionExtensions.generated.cs", expectedResult)
                    }
                },
            }.RunAsync();
        }
    }
}
