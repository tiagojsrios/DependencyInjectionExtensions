using DependencyInjectionExtensions.TestClass.Interfaces;

namespace DependencyInjectionExtensions.TestClass.Repositories
{
    [DependencyInjectionExtensions.Attributes.ServiceDescriptor("Scoped", Type = typeof(IGenericRepository<>))]
    public class GenericRepository<T> : IGenericRepository<T> {}
}