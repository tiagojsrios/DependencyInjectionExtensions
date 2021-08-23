using DependencyInjectionExtensions.TestClass.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionExtensions.TestClass.Repositories
{
    [DependencyInjectionExtensions.Attributes.ServiceDescriptor(ServiceLifetime.Scoped, Type = typeof(IGenericRepository<>))]
    public class GenericRepository<T> : IGenericRepository<T> {}
}