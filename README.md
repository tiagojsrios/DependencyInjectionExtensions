# DependencyInjectionExtensions
When developing a Web API, developers face the repetitive task of having to register Services or Repositories to the dependency injection container. Therefore, the goal of this project is to reduce this overhead that developers have.

# How to use [ServiceDescriptorAttribute]
This attribute is based on [Scrutor](https://github.com/khellang/Scrutor), hence the idea is pretty much the same. If you aren't familiar with Scrutor, the following code snippet should give you an idea on how to use the attribute.

```csharp
public interface IUserRepository {}

namespace My.Namespace.Repositories
{
    [ServiceDescriptor(ServiceLifetime.Singleton, Type = typeof(IUserRepository))]
    public class UserRepository : IUserRepository {}
}
```

This will generate the following method:

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

<h2><u>What does this attribute support?</u></h2>
<details>
    <summary>Click to expand!</summary>

1. **Registration of open generics;**

```csharp
public interface IUserRepository {}

[ServiceDescriptor(ServiceLifetime.Singleton, Type = typeof(IUserRepository<>))]
public class UserRepository<T> : IUserRepository<T> {}
```

2. **Registration of a class that implements a generic interface;**

```csharp
public interface IUserRepository {}

[ServiceDescriptor(ServiceLifetime.Singleton, Type = typeof(IUserRepository<string>))]
public class UserRepository : IUserRepository<string> {}
```

3. **Registration of a class that implements an interface;**

```csharp
public interface IUserRepository {}

[ServiceDescriptor(ServiceLifetime.Singleton, Type = typeof(IUserRepository))]
public class UserRepository : IUserRepository {}
```

4. **Registration of a simple class.**

```csharp
[ServiceDescriptor(ServiceLifetime.Singleton)]
public class UserRepository {}
```
</details>
<br/>

# HttpClientAttribute

The goal is to register in the `IServiceCollection` a typed `HttpClient`. However, it's still part of the roadmap.
<br/>

# OptionsAttribute

The goal is to register in the `IServiceCollection` an `IOptions<T>` object. However, it's still part of the roadmap.