using DependencyInjectionExtensions.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionExtensions.Tests
{
    public interface IMultipleRegistrationsInterface { }

    [ServiceDescriptor(ServiceLifetime.Singleton)]
    [ServiceDescriptor(ServiceLifetime.Scoped)]
    public class MultipleRegistrations : IMultipleRegistrationsInterface
    {
    }
}