using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionExtensions.TestClass.Repositories
{
    [DependencyInjectionExtensions.Attributes.ServiceDescriptor(ServiceLifetime.Singleton)]
    public class NoInterfaceBaseRepository
    {
    }
}
