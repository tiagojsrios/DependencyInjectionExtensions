using DependencyInjectionExtensions.TestClass.Interfaces;

namespace DependencyInjectionExtensions.TestClass.Repositories
{
    [DependencyInjectionExtensions.Attributes.ServiceDescriptor("Singleton", Type = typeof(IBaseRepository))]
    public class BaseRepository : IBaseRepository {}
}