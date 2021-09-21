using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TestProject.Extensions
{
    public static partial class ServiceCollectionExtensions
    {
        internal static IServiceCollection RegisterServices(this IServiceCollection services) => services.RegisterServicesForTestProject();
        public static IServiceCollection RegisterServicesForTestProject(this IServiceCollection services)
        {
            services.TryAddTransient<DependencyInjectionExtensions.Tests.IMultipleInterfacesTransientRegistrationInterface1, DependencyInjectionExtensions.Tests.MultipleInterfacesTransientRegistration>();
            services.TryAddTransient<DependencyInjectionExtensions.Tests.IMultipleInterfacesTransientRegistrationInterface2, DependencyInjectionExtensions.Tests.MultipleInterfacesTransientRegistration>();
            services.TryAddScoped<DependencyInjectionExtensions.Tests.IMultipleInterfacesScopedRegistrationInterface1, DependencyInjectionExtensions.Tests.MultipleInterfacesScopedRegistration>();
            services.TryAddScoped(typeof(DependencyInjectionExtensions.Tests.IMultipleInterfacesScopedRegistrationInterface2), sp => sp.GetService(typeof(DependencyInjectionExtensions.Tests.IMultipleInterfacesScopedRegistrationInterface1)));
            return services;
        }
    }
}