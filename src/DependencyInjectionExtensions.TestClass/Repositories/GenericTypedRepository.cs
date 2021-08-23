using DependencyInjectionExtensions.TestClass.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionExtensions.TestClass.Repositories
{
    [DependencyInjectionExtensions.Attributes.ServiceDescriptor(ServiceLifetime.Transient, Type = typeof(IGenericTypedRepository<string>))]
    public class GenericTypedRepository : IGenericTypedRepository<string> { }
}
