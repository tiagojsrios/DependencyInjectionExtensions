using DependencyInjectionExtensions.Sample.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionExtensions.Sample.Repositories
{
    [DependencyInjectionExtensions.Attributes.ServiceDescriptor(ServiceLifetime.Scoped, Type = typeof(IGenericRepository<>))]
    public class GenericRepository<T> : IGenericRepository<T> {}
}