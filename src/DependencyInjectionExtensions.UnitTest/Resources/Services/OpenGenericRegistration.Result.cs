using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TestProject.Extensions
{
    public static partial class ServiceCollectionExtensions
    {
        internal static IServiceCollection RegisterServices(this IServiceCollection services) => services.RegisterServicesForTestProject();
        public static IServiceCollection RegisterServicesForTestProject(this IServiceCollection services)
        {
            services.TryAddTransient(typeof(DependencyInjectionExtensions.Tests.IOpenGenericInterface<, >), typeof(DependencyInjectionExtensions.Tests.OpenGenericRegistration<, >));
            return services;
        }
    }
}