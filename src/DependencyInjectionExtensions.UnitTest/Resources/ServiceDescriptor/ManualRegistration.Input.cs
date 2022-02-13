using DependencyInjectionExtensions.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionExtensions.Tests
{
    public interface ISimpleInterface1 { }
    public interface ISimpleSkippedInterface2 { }
    public interface ISimpleInterface3 { }

    [ServiceDescriptor(ServiceLifetime.Transient, typeof(ISimpleInterface1), typeof(ISimpleInterface3)  )]
    public class ManualRegistration : ISimpleInterface1, ISimpleSkippedInterface2, ISimpleInterface3
    {
    }
}