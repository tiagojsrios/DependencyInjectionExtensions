using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TestProject.Extensions
{
    public static partial class ServiceCollectionExtensions
    {
        internal static IServiceCollection RegisterServices(this IServiceCollection services) => services.RegisterServicesForTestProject();
        public static IServiceCollection RegisterServicesForTestProject(this IServiceCollection services)
        {
            services.TryAdd_Lifetime_<DependencyInjectionExtensions.Tests.ISimpleServiceRegistrationInterface, DependencyInjectionExtensions.Tests.SimpleServiceRegistration>();
            return services;
        }
    }
}