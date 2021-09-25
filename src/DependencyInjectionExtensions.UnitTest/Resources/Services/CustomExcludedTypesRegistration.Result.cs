using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TestProject.Extensions
{
    public static partial class ServiceCollectionExtensions
    {
        internal static IServiceCollection RegisterServices(this IServiceCollection services) => services.RegisterServicesForTestProject();
        public static IServiceCollection RegisterServicesForTestProject(this IServiceCollection services)
        {
            services.TryAddTransient<DependencyInjectionExtensions.Tests.ISimpleInterface1, DependencyInjectionExtensions.Tests.CustomExcludedTypesRegistration>();
            services.TryAddTransient<DependencyInjectionExtensions.Tests.ISimpleInterface3, DependencyInjectionExtensions.Tests.CustomExcludedTypesRegistration>();
            return services;
        }
    }
}