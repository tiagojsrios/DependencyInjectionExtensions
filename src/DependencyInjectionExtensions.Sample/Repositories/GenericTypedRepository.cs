using DependencyInjectionExtensions.Sample.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionExtensions.Sample.Repositories
{
    [DependencyInjectionExtensions.Attributes.ServiceDescriptor(ServiceLifetime.Transient, Type = typeof(IGenericTypedRepository<string>))]
    public class GenericTypedRepository : IGenericTypedRepository<string> { }
}
