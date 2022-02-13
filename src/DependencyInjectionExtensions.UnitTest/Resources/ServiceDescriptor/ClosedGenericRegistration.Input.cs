using DependencyInjectionExtensions.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionExtensions.Tests
{
    public interface IGenericInterface<T1,T2> { }

    [ServiceDescriptor(ServiceLifetime.Scoped)]
    public class ClosedGenericRegistration : IGenericInterface<object, object>
    {
    }
}