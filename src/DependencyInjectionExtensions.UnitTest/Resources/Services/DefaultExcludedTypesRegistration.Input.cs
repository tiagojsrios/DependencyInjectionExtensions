using System;
using System.Runtime.Serialization;
using DependencyInjectionExtensions.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionExtensions.Tests
{
    public interface ISimpleInterface { }

    [ServiceDescriptor(ServiceLifetime.Transient)]
    public class DefaultExcludedTypesRegistration : ISimpleInterface, IDisposable, ISerializable
    {
        public void Dispose() => throw new NotImplementedException();

        public void GetObjectData(SerializationInfo info, StreamingContext context) => throw new NotImplementedException();
    }
}