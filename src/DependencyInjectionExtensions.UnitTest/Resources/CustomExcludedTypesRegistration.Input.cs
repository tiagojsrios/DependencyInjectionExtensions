using DependencyInjectionExtensions.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DependencyInjectionExtensions.Tests
{
    public interface ISimpleInterface1 { }
    public interface ISimpleSkippedInterface2 { }
    public interface ISimpleInterface3 { }

    [ServiceDescriptor(ServiceLifetime.Transient, ExcludedTypes = new Type[] { typeof(ISimpleSkippedInterface2) } )]
    public class CustomExcludedTypesRegistration : ISimpleInterface1, ISimpleSkippedInterface2, ISimpleInterface3
    {
    }
}