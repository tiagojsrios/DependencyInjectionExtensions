using DependencyInjectionExtensions.Sample.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionExtensions.Sample.Repositories
{
    [DependencyInjectionExtensions.Attributes.ServiceDescriptor(ServiceLifetime.Singleton)]
    public class BaseRepository : IBaseRepository { }
}