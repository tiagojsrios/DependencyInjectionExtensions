using DependencyInjectionExtensions.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionExtensions.Tests
{
    public interface ISimpleInterface { }

    [ServiceDescriptor(ServiceLifetime.Singleton, RegistrationTypeSelectors.SelfAndImplementedInterfaces )]
    public class ImplementedInterfacesAndSelfRegistration : ISimpleInterface
    {
    }
}