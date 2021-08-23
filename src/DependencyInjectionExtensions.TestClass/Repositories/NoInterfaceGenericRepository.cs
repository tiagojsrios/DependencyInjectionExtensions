using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionExtensions.TestClass.Repositories
{
    [DependencyInjectionExtensions.Attributes.ServiceDescriptor(ServiceLifetime.Transient)]
    public class NoInterfaceGenericRepository<T>
    {
    }
}
