using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TestProject.Extensions
{
    public static partial class ServiceCollectionExtensions
    {
        internal static IServiceCollection RegisterServices(this IServiceCollection services) => services.RegisterServicesForTestProject();
        public static IServiceCollection RegisterServicesForTestProject(this IServiceCollection services)
        {
            services.TryAddSingleton<DependencyInjectionExtensions.Tests.IMultipleRegistrationsInterface, DependencyInjectionExtensions.Tests.MultipleRegistrations>();
            services.TryAddScoped<DependencyInjectionExtensions.Tests.IMultipleRegistrationsInterface, DependencyInjectionExtensions.Tests.MultipleRegistrations>();
            return services;
        }
    }
}