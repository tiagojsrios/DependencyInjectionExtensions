using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TestProject.Extensions
{
    public static partial class ServiceCollectionExtensions
    {
        internal static IServiceCollection RegisterServices(this IServiceCollection services) => services.RegisterServicesForTestProject();
        public static IServiceCollection RegisterServicesForTestProject(this IServiceCollection services)
        {
            services.TryAddSingleton<DependencyInjectionExtensions.Tests.ImplementedInterfacesAndSelfRegistration, DependencyInjectionExtensions.Tests.ImplementedInterfacesAndSelfRegistration>();
            services.TryAddSingleton(typeof(DependencyInjectionExtensions.Tests.ISimpleInterface), sp => sp.GetService(typeof(DependencyInjectionExtensions.Tests.ImplementedInterfacesAndSelfRegistration)));
            return services;
        }
    }
}