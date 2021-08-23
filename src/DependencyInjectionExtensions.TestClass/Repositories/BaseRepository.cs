using DependencyInjectionExtensions.TestClass.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionExtensions.TestClass.Repositories
{
    [DependencyInjectionExtensions.Attributes.ServiceDescriptor(ServiceLifetime.Singleton, Type = typeof(IBaseRepository))]
    public class BaseRepository : IBaseRepository {}
}