using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> Services = new();

    public static void Register<T>(T service)
    {
        Type serviceType = typeof(T);

        if (Services.ContainsKey(serviceType))
        {
            throw new InvalidOperationException(
                $"Service of type {serviceType.Name} is already registered."
            );
        }

        Services.Add(serviceType, service);
    }

    public static T Locate<T>()
    {
        Type serviceType = typeof(T);

        if (!Services.TryGetValue(serviceType, out object service))
        {
            throw new InvalidOperationException(
                $"Service of type {serviceType.Name} is not registered."
            );
        }

        return (T)service;
    }

    public static bool TryGet<T>(out T service)
    {
        if (Services.TryGetValue(typeof(T), out object result))
        {
            service = (T)result;
            return true;
        }

        service = default;
        return false;
    }

    public static void Unregister<T>()
    {
        Services.Remove(typeof(T));
    }

    public static bool IsRegistered<T>()
    {
        return Services.ContainsKey(typeof(T));
    }

    public static void Clear()
    {
        Services.Clear();
    }
}