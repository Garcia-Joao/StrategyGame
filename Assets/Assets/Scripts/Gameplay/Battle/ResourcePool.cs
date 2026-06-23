using System;

[Serializable]
public class ResourcePool
{
    public int Max { get; private set; }

    public int Current { get; private set; }

    public event Action Changed;

    public void Initialize(int value)
    {
        Max = value;
        Current = value;

        Changed?.Invoke();
    }

    public bool Has(int value)
    {
        return Current >= value;
    }

    public void Consume(int value)
    {
        Current = Math.Max(
            0,
            Current - value);

        Changed?.Invoke();
    }

    public void Restore(int value)
    {
        Current = Math.Min(
            Max,
            Current + value);

        Changed?.Invoke();
    }

    public void Reset()
    {
        Current = Max;

        Changed?.Invoke();
    }
}