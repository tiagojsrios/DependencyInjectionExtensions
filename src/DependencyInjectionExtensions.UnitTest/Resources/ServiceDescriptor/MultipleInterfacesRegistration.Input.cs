using DependencyInjectionExtensions.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionExtensions.Tests
{
    public interface IMultipleInterfacesTransientRegistrationInterface1 { }
    public interface IMultipleInterfacesTransientRegistrationInterface2 { }

    [ServiceDescriptor(ServiceLifetime.Transient)]
    public class MultipleInterfacesTransientRegistration : IMultipleInterfacesTransientRegistrationInterface1, IMultipleInterfacesTransientRegistrationInterface2
    {
    }

    public interface IMultipleInterfacesScopedRegistrationInterface1 { }
    public interface IMultipleInterfacesScopedRegistrationInterface2 { }

    [ServiceDescriptor(ServiceLifetime.Scoped)]
    public class MultipleInterfacesScopedRegistration : IMultipleInterfacesScopedRegistrationInterface1, IMultipleInterfacesScopedRegistrationInterface2
    {
    }
}