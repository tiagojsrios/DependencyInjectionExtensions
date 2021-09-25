using DependencyInjectionExtensions.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionExtensions.Tests
{
    public interface ISimpleServiceRegistrationInterface { }

    [ServiceDescriptor(ServiceLifetime._Lifetime_)]
    public class SimpleServiceRegistration : ISimpleServiceRegistrationInterface
    {
    }
}