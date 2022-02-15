# DependencyInjectionExtensions
When developing a Web API, developers face the repetitive task of having to register Services or Repositories to the dependency injection container. Therefore, the goal of this project is to reduce this overhead that developers have.

# ServiceDescriptorAttribute
This attribute is based on [Scrutor](https://github.com/khellang/Scrutor), hence the idea is pretty much the same. If you aren't familiar with Scrutor, the following code snippet should give you an idea on how to use the attribute.

```csharp
public interface IUserRepository {}

namespace My.Namespace.Repositories
{
    [ServiceDescriptor(ServiceLifetime.Singleton, Type = typeof(IUserRepository))]
    public class UserRepository : IUserRepository {}
}
```

This will generate the following code:

```csharp
namespace My.Namespace.Extensions
{
    public static partial class ServiceCollectionExtensions
    {
        internal static IServiceCollection RegisterServices(this IServiceCollection services) => services.RegisterServicesForMyNamespace();
        
        public static IServiceCollection RegisterServicesForMyNamespace(this IServiceCollection services)
        {
            services.AddSingleton<IUserRepository, UserRepository>();
            return services;
        }
    }
}
```

`ServiceCollectionExtensions`' namespace will be the assembly's namespace, where `UserRepository` is located, concatenated with ".Extensions". In our example, assuming that `My.Namespace` is the assembly's namespace, the end result will be `My.Namespace.Extensions`. 

# OptionsAttribute
Another common practice while developing .NET applications is to create binding classes. This allows you to configure object properties' values in `appsettings.json`, create and add an object to the dependency injection container as a `IOptions<T>`.

```csharp
namespace My.Namespace;

[Options]
public class DatabaseContextOptions
{
    [Required] public string ConnectionString { get; set; }
}
```

This will generate the following code:

```csharp
namespace My.Namespace.Extensions
{
    public static partial class ServiceCollectionExtensions
    {
        internal static IServiceCollection RegisterOptions(this IServiceCollection services) => services.RegisterOptionsForMyNamespace();
        
        public static IServiceCollection RegisterOptionsForMyNamespace(this IServiceCollection services)
        {
            services.AddOptions<My.Namespace.DatabaseContextOptions>().Bind(configuration.GetSection("DatabaseContextOptions"));
            return services;
        }
    }
}
```