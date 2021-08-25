using DependencyInjectionExtensions.Sample.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionExtensions.Sample.Repositories
{
    [DependencyInjectionExtensions.Attributes.ServiceDescriptor(ServiceLifetime.Singleton, Type = typeof(IBaseRepository))]
    public class BaseRepository : IBaseRepository {}
}