using System;
using System.Collections.Generic;

public sealed class GameContainer
{
    private readonly Dictionary<Type, object> services = new();

    public void Register<T>(T instance)
    {
        services[typeof(T)] = instance;
    }

    public T Resolve<T>()
    {
        return (T)services[typeof(T)];
    }
}