using DependencyInjectionExtensions.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionExtensions.Tests
{
    public interface IOpenGenericInterface<T1,T2> { }

    [ServiceDescriptor(ServiceLifetime.Transient)]
    public class OpenGenericRegistration<T1, T2> : IOpenGenericInterface<T1, T2>
    {
    }
}