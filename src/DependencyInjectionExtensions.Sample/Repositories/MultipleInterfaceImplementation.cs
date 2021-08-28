using DependencyInjectionExtensions.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionExtensions.Sample.Repositories
{
    public interface ISkipInterfaceImplementation { }

    public interface IMultipleInterfaceImplementation { }

    public interface IAnotherMultipleInterfaceImplementation { }

    [ServiceDescriptor(ServiceLifetime.Scoped, ExcludedTypes = new[] { typeof(ISkipInterfaceImplementation) })]
    public class MultipleInterfaceImplementation : IMultipleInterfaceImplementation, IAnotherMultipleInterfaceImplementation, ISkipInterfaceImplementation
    {
    }
}
