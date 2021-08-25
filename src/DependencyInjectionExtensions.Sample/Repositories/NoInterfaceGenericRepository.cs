using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionExtensions.Sample.Repositories
{
    [DependencyInjectionExtensions.Attributes.ServiceDescriptor(ServiceLifetime.Transient)]
    public class NoInterfaceGenericRepository<T>
    {
    }
}
