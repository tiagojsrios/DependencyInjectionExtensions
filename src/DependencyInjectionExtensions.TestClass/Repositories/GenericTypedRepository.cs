using DependencyInjectionExtensions.TestClass.Interfaces;

namespace DependencyInjectionExtensions.TestClass.Repositories
{
    [DependencyInjectionExtensions.Attributes.ServiceDescriptor("Transient", Type = typeof(IGenericTypedRepository<string>))]
    public class GenericTypedRepository : IGenericTypedRepository<string> { }
}
